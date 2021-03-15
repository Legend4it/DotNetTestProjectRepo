using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarPRNTSDK
{
    public partial class CombinationExtSamplePage : Page
    {
        public enum PrinterStatus
        {
            Invalid,
            Impossible,
            Online,
            Offline
        }

        public enum PaperStatus
        {
            Invalid,
            Ready,
            NearEmpty,
            Empty
        }

        public enum CoverStatus
        {
            Invalid,
            Open,
            Close
        }

        public enum CashDrawerStatus
        {
            Invalid,
            Open,
            Close
        }

        private IPort port;
        private Thread monitoringPrinterThread;
        private bool cancellationPending;
        private object lockObject;
        private PrinterStatus currentPrinterStatus;
        private PaperStatus currentPaperStatus;
        private CoverStatus currentCoverStatus;
        private CashDrawerStatus currentCashDrawerStatus;
        private Communication.PeripheralStatus currentBarcodeReaderStatus;


        /// <summary>
        /// Sample : Starting monitoring printer.
        /// </summary>
        public void Connect()
        {
            try
            {
                if (port == null)
                {
                    // Your printer PortName and PortSettings.
                    string portName = SharedInformationManager.SelectedPortName;
                    string portSettings = SharedInformationManager.SelectedPortSettings;

                    port = Factory.I.GetPort(portName, portSettings, 10000);

                    // First, clear barcode reader buffer.
                    ClearBarcodeReaderBuffer();
                }
            }
            catch (PortException) // Port open is failed.
            {
                DidConnectFailed();

                return;
            }

            try
            {
                if (monitoringPrinterThread == null || monitoringPrinterThread.ThreadState == ThreadState.Stopped)
                {
                    monitoringPrinterThread = new Thread(MonitoringPrinter);
                    monitoringPrinterThread.Name = "MonitoringPrinterThread";
                    monitoringPrinterThread.IsBackground = true;
                    monitoringPrinterThread.Start();
                }
            }
            catch (Exception) // Start monitoring printer thread is failure.
            {
                DidConnectFailed();
            }

            currentPrinterStatus = PrinterStatus.Invalid;
            currentPaperStatus = PaperStatus.Invalid;
            currentCoverStatus = CoverStatus.Invalid;
            currentCashDrawerStatus = CashDrawerStatus.Invalid;
            currentBarcodeReaderStatus = Communication.PeripheralStatus.Invalid;
        }

        /// <summary>
        /// Sample : Stoping monitoring printer.
        /// </summary>
        public void Disconnect()
        {
            bool isThreadStopped = StopMonitoringThread(); // Stop monitoring thread.

            if (isThreadStopped)
            {
                if (port != null) // Release port.
                {
                    Factory.I.ReleasePort(port);

                    port = null;
                }
            }
        }

        private bool StopMonitoringThread()
        {
            if (monitoringPrinterThread == null)
            {
                return true;
            }

            bool result = false;

            try
            {
                if ((monitoringPrinterThread.ThreadState & (ThreadState.Aborted | ThreadState.Stopped)) == 0)
                {
                    cancellationPending = true;

                    if (!monitoringPrinterThread.Join(TimeSpan.FromSeconds(5)))
                    {
                        monitoringPrinterThread.Abort();

                        if (!monitoringPrinterThread.Join(TimeSpan.FromSeconds(5)))
                        {
                            throw new Exception("Stopping thread is failed.");
                        }
                    }
                }

                monitoringPrinterThread = null;

                result = true; // Success.
            }
            catch (Exception) { }

            cancellationPending = false;

            return result;
        }

        /// <summary>
        /// Sample : Monitoring printer process.
        /// </summary>
        private void MonitoringPrinter()
        {
            // Your printer emulation.            
            Emulation emulation = SharedInformationManager.SelectedEmulation;

            while (!cancellationPending)
            {
                lock (lockObject)
                {
                    try
                    {
                        if (port != null)
                        {
                            StarPrinterStatus status = port.GetParsedStatus();

                            CheckPrinterStatus(status); // Check printer status.

                            CheckPaperStatus(status); // Check paper status.

                            CheckCoverStatus(status); // Check cover status.

                            CheckCashDrawer(status); // Check cash drawer status.

                            CheckBarcodeReaderStatus(); // Check barcode reader status. (connect or disconnect)

                            if (currentBarcodeReaderStatus == Communication.PeripheralStatus.Connect) // Barcode reader is connected.
                            {
                                ReadBarcodeReaderData();  // Read barcode reader data.
                            }
                        }
                    }
                    catch (PortException)
                    {
                        OnPrinterImpossible();
                    }
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Sample : Creating printing receipt and open cash drawer commands.
        /// </summary>
        private byte[] CreateLocalizeReceiptAndOpenCashDrawerCommands(ReceiptInformationManager receiptInfo)
        {
            // Your printer emulation.
            Emulation emulation = SharedInformationManager.SelectedEmulation;

            // print paper size
            int paperSize = SharedInformationManager.SelectedActualPaperSize;

            // Creating localize receipt commands sample is in "LocalizeReceipts/'Language'Receipt.cs"
            ReceiptInformationManager.ReceiptType type = receiptInfo.Type;
            LocalizeReceipt localizeReceipt = receiptInfo.LocalizeReceipt;

            byte[] commands;

            switch (type)
            {
                default:
                case ReceiptInformationManager.ReceiptType.Text:
                    commands = CombinationFunctions.CreateTextReceiptData(emulation, localizeReceipt, false);
                    break;

                case ReceiptInformationManager.ReceiptType.TextUTF8:
                    commands = CombinationFunctions.CreateTextReceiptData(emulation, localizeReceipt, true);
                    break;

                case ReceiptInformationManager.ReceiptType.Raster:
                    commands = CombinationFunctions.CreateRasterReceiptData(emulation, localizeReceipt);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterBothScale:
                    commands = CombinationFunctions.CreateScaleRasterReceiptData(emulation, localizeReceipt, paperSize, true);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterScale:
                    commands = CombinationFunctions.CreateScaleRasterReceiptData(emulation, localizeReceipt, paperSize, false);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterCoupon:
                    commands = CombinationFunctions.CreateCouponData(emulation, localizeReceipt, paperSize, BitmapConverterRotation.Normal);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterCouponRotation90:
                    commands = CombinationFunctions.CreateCouponData(emulation, localizeReceipt, paperSize, BitmapConverterRotation.Right90);
                    break;
            }

            return commands;
        }

        private void ClearBarcodeReaderBuffer()
        {
            byte[] clearBufferCommands = BcrFunctions.CreateClearBuffer();

            port.WritePort(clearBufferCommands, 0, (uint)clearBufferCommands.Length);
        }

        private void CheckPrinterStatus(StarPrinterStatus status)
        {
            if (status.Offline) // Printer offline
            {
                OnPrinterOffline();
            }
            else                // Printer online
            {
                OnPrinterOnline();
            }
        }

        private void CheckPaperStatus(StarPrinterStatus status)
        {
            if (status.ReceiptPaperEmpty)                 // Paper empty
            {
                OnPrinterPaperEmpty();
            }
            else if (status.ReceiptPaperNearEmptyInner || // Paper near empty
                     status.ReceiptPaperNearEmptyOuter)
            {
                OnPrinterPaperNearEmpty();
            }
            else                                          // Paper ready
            {
                OnPrinterPaperReady();
            }
        }

        private void CheckCoverStatus(StarPrinterStatus status)
        {
            if (status.CoverOpen) // Cover open
            {
                OnPrinterCoverOpen();
            }
            else                  // Cover close
            {
                OnPrinterCoverClose();
            }
        }

        private void CheckCashDrawer(StarPrinterStatus status)
        {
            // Your printer cash drawer open status.
            bool cashDrawerOpenActiveHigh = SharedInformationManager.SelectedDrawerOpenStatus;

            if (status.CompulsionSwitch == cashDrawerOpenActiveHigh) // Cash drawer open
            {
                OnCashDrawerOpen();

            }
            else
            {
                OnCashDrawerClose();                                 // Cash drawer close
            }
        }

        /// <summary>
        /// Sample : Checking barcode reader status.
        /// </summary>
        private void CheckBarcodeReaderStatus()
        {
            // Create IPeripheralConnectParser object.
            IPeripheralConnectParser parser = StarIoExt.CreateBcrConnectParser(BcrModel.POP1);

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);

            // Check peripheral status.
            currentBarcodeReaderStatus = Communication.PeripheralStatus.Invalid;
            if (result.Result == Communication.Result.Success)
            {
                // Check parser property value.
                if (parser.IsConnected) // connect
                {
                    currentBarcodeReaderStatus = Communication.PeripheralStatus.Connect;
                }
                else // disconnect
                {
                    currentBarcodeReaderStatus = Communication.PeripheralStatus.Disconnect;
                }
            }
            else // communication error
            {
                currentBarcodeReaderStatus = Communication.PeripheralStatus.Impossible;
            }

            switch (currentBarcodeReaderStatus)
            {
                default:
                case Communication.PeripheralStatus.Impossible:
                    OnPrinterImpossible();
                    break;

                case Communication.PeripheralStatus.Connect:
                    OnBarcodeReaderConnect();
                    break;

                case Communication.PeripheralStatus.Disconnect:
                    OnBarcodeReaderDisconnect();
                    break;
            }
        }

        /// <summary>
        /// Sample : Reading barcode reader data.
        /// </summary>
        private void ReadBarcodeReaderData()
        {
            // Barcode reader data parser sample is in "BcrDataParser" class.
            BcrDataParser parser = new BcrDataParser();

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);

            if (result.Result != Communication.Result.Success)
            {
                OnPrinterImpossible();

                return;
            }

            // Check parser property value.
            byte[] barcodeData = parser.Data;

            if (barcodeData != null) // Barcode reader data is not empty.
            {
                OnBarcodeDataReceived(barcodeData);
            }
        }

        private void DidConnectFailed()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                statusTextBlock.Text = "Check the device. (Power and Bluetooth pairing)\nThen touch up the Refresh button.";
                statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            })
            );
        }

        private void OnPrinterImpossible()
        {
            if (currentPrinterStatus != PrinterStatus.Impossible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Printer Impossible.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                })
                );
            }

            currentPrinterStatus = PrinterStatus.Impossible;
        }

        private void OnPrinterOnline()
        {
            if (currentPrinterStatus != PrinterStatus.Online)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Printer Online.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
                })
                );
            }

            currentPrinterStatus = PrinterStatus.Online;
        }

        private void OnPrinterOffline()
        {
            if (currentPrinterStatus != PrinterStatus.Offline)
            {
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    statusTextBlock.Text = "Printer Offline.";
                //    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                //})
                //);
            }

            currentPrinterStatus = PrinterStatus.Offline;
        }

        private void OnPrinterPaperNearEmpty()
        {
            if (currentPaperStatus != PaperStatus.NearEmpty)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Printer Paper Near Empty.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Orange);
                })
                );
            }

            currentPaperStatus = PaperStatus.NearEmpty;
        }

        private void OnPrinterPaperEmpty()
        {
            if (currentPaperStatus != PaperStatus.Empty)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Printer Paper Empty.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                })
                );
            }

            currentPaperStatus = PaperStatus.Empty;
        }

        private void OnPrinterPaperReady()
        {
            if (currentPaperStatus != PaperStatus.Ready)
            {
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    statusTextBlock.Text = "Printer Paper Ready.";
                //    statusTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
                //})
                //);
            }

            currentPaperStatus = PaperStatus.Ready;
        }

        private void OnPrinterCoverOpen()
        {
            if (currentCoverStatus != CoverStatus.Open)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Printer Cover Open.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                })
                );
            }

            currentCoverStatus = CoverStatus.Open;
        }

        private void OnPrinterCoverClose()
        {
            if (currentCoverStatus != CoverStatus.Close)
            {
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    statusTextBlock.Text = "Printer Cover Close.";
                //    statusTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
                //})
                //);
            }

            currentCoverStatus = CoverStatus.Close;
        }

        private void OnCashDrawerOpen()
        {
            if (currentCashDrawerStatus != CashDrawerStatus.Open)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Cash Drawer Open.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Magenta);
                })
                );
            }

            currentCashDrawerStatus = CashDrawerStatus.Open;
        }


        private void OnCashDrawerClose()
        {
            if (currentCashDrawerStatus != CashDrawerStatus.Close)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Cash Drawer Close.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
                })
                );
            }

            currentCashDrawerStatus = CashDrawerStatus.Close;
        }

        private void OnBarcodeDataReceived(byte[] receivedData)
        {
            string text = Encoding.UTF8.GetString(receivedData, 0, receivedData.Length);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                AddTextToList(text);
            })
            );
        }

        private void OnBarcodeReaderConnect()
        {
            if (currentBarcodeReaderStatus != Communication.PeripheralStatus.Connect)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Barcode Reader Connect.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Blue);
                })
                );
            }

            currentBarcodeReaderStatus = Communication.PeripheralStatus.Connect;
        }

        private void OnBarcodeReaderDisconnect()
        {
            if (currentBarcodeReaderStatus != Communication.PeripheralStatus.Disconnect)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusTextBlock.Text = "Barcode Reader Disconnect.";
                    statusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                })
                );
            }

            currentBarcodeReaderStatus = Communication.PeripheralStatus.Disconnect;
        }

        private void AddTextToList(string text)
        {
            string[] splittedTextArray = text.Split('\n');

            foreach (var splittedText in splittedTextArray)
            {
                if (!splittedText.Equals(""))
                {
                    string barcode = splittedText;

                    int index = splittedText.IndexOf("\r");

                    if (index != -1)
                    {
                        barcode = barcode.Substring(0, index);
                    }

                    readDataListBox.Items.Add(barcode);

                    pageScrollViewer.ScrollToBottom();
                }
            }
        }

        private void ClearTextFromList()
        {
            readDataListBox.Items.Clear();

            pageScrollViewer.ScrollToBottom();
        }

        private void PrintReceiptAndOpenCashDrawer()
        {
            lock (lockObject)
            {
                ReceiptInformationManager receiptInfo = SharedInformationManager.ReceiptInformationManager;

                byte[] commands = CreateLocalizeReceiptAndOpenCashDrawerCommands(receiptInfo);

                Communication.SendCommandsWithProgressBar(commands, port);
            }
        }

        private void ConnectWithProgressBar()
        {
            ProgressBarWindow progressBarWindow = new ProgressBarWindow("Communicating...", () =>
            {
                Connect();
            });

            progressBarWindow.ShowDialog();
        }

        private void DisconnectWithProgressBar()
        {
            ProgressBarWindow progressBarWindow = new ProgressBarWindow("Disconneting Printer...", () =>
            {
                Disconnect();
            });

            progressBarWindow.ShowDialog();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectWithProgressBar();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DisconnectWithProgressBar();
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectWithProgressBar();

            ClearTextFromList();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintReceiptAndOpenCashDrawer();
        }

        public CombinationExtSamplePage()
        {
            InitializeComponent();

            lockObject = new object();
        }
    }
}
