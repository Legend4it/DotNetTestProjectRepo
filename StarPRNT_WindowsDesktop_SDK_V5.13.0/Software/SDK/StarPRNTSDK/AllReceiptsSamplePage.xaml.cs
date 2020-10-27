using Microsoft.Win32;
using StarMicronics.SMCloudServicesSolution;
using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace StarPRNTSDK
{
    public static class AllReceiptsSampleManager
    {
        /// <summary>
        /// Sample : Showing AllRceipts registration view.
        /// </summary>
        public static void ShowAllReceiptsRegistrationView()
        {
            // Check is your device is registered.
            bool isAlreadyRegistered = CheckIsDeviceRegisteredInStarCloudServices();

            if (!isAlreadyRegistered) // Not registered
            {
                // Specify the file path to save the registration information. (XML format)
                string path = SetRegistrationConfigFilePath();

                if (path == null)
                {
                    return;
                }

                // Please be sure to set registration config path first.
                SMCloudServices.SetRegistrationConfigFilePath(path);
            }

            // Specify owner window. (option)
            Window ownerWindow = Util.GetMainWindow();

            SMCloudServices.ShowRegistrationView(ownerWindow);

            // If without owner window, use below method.
            // SMCloudServices.ShowRegistrationView(); 
        }

        /// <summary>
        /// Sample : Creating printing receipt with AllReceipts commands.
        /// </summary>
        public static byte[] CreateLocalizeReceiptWithAllReceiptsCommands(ReceiptInformationManager receiptInfo)
        {
            byte[] commands;

            // Your printer emulation.
            Emulation emulation = SharedInformationManager.SelectedEmulation;

            // print paper size
            int paperSize = receiptInfo.ActualPaperSize;

            // Creating localize receipt commands sample is in "LocalizeReceipts/'Language'Receipt.cs"
            ReceiptInformationManager.ReceiptType type = receiptInfo.Type;
            LocalizeReceipt localizeReceipt = receiptInfo.LocalizeReceipt;

            // Select printing contents.
            bool printReceipt = SharedInformationManager.AllReceiptsPrintReceipt;
            bool printInformation = SharedInformationManager.AllReceiptsPrintInformation;
            bool printQrCode = SharedInformationManager.AllReceiptsPrintQrCode;

            switch (type)
            {
                default:
                case ReceiptInformationManager.ReceiptType.Text:
                    commands = AllReceiptsFunctions.CreateTextReceiptData(emulation, localizeReceipt, paperSize, false, printReceipt, printInformation, printQrCode);
                    break;

                case ReceiptInformationManager.ReceiptType.TextUTF8:
                    commands = AllReceiptsFunctions.CreateTextReceiptData(emulation, localizeReceipt, paperSize, true, printReceipt, printInformation, printQrCode);
                    break;

                case ReceiptInformationManager.ReceiptType.Raster:
                    commands = AllReceiptsFunctions.CreateRasterReceiptData(emulation, localizeReceipt, paperSize, printReceipt, printInformation, printQrCode);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterBothScale:
                    commands = AllReceiptsFunctions.CreateScaleRasterReceiptData(emulation, localizeReceipt, paperSize, true, printReceipt, printInformation, printQrCode);
                    break;

                case ReceiptInformationManager.ReceiptType.RasterScale:
                    commands = AllReceiptsFunctions.CreateScaleRasterReceiptData(emulation, localizeReceipt, paperSize, false, printReceipt, printInformation, printQrCode);
                    break;

            }

            return commands;
        }


        /// <summary>
        /// Sample : Setting AllReceipts function event.
        /// </summary>
        public static void AddAllReceiptsFunctionEvent()
        {
            SMCloudServices.RegisteredEvent += AllReceiptsFunctions.SMCloudServices_RegisterResultEvent;

            SMCSAllReceipts.UpdatedStatusResultEvent += AllReceiptsFunctions.SMCSAllReceipts_UpdatedStatusResultEvent;
            SMCSAllReceipts.UploadedBitmapResultEvent += AllReceiptsFunctions.SMCSAllReceipts_UploadedBitmapResultEvent;
            SMCSAllReceipts.UploadedDataResultEvent += AllReceiptsFunctions.SMCSAllReceipts_UploadedDataResultEvent;
        }

        /// <summary>
        /// Sample : Removing AllReceipts function event.
        /// </summary>
        public static void RemoveAllReceiptsFunctionEvent()
        {
            SMCloudServices.RegisteredEvent -= AllReceiptsFunctions.SMCloudServices_RegisterResultEvent;

            SMCSAllReceipts.UpdatedStatusResultEvent -= AllReceiptsFunctions.SMCSAllReceipts_UpdatedStatusResultEvent;
            SMCSAllReceipts.UploadedBitmapResultEvent -= AllReceiptsFunctions.SMCSAllReceipts_UploadedBitmapResultEvent;
            SMCSAllReceipts.UploadedDataResultEvent -= AllReceiptsFunctions.SMCSAllReceipts_UploadedDataResultEvent;
        }

        /// <summary>
        /// Sample : Checking is your device is registered in StarCloudServices.
        /// </summary>
        private static bool CheckIsDeviceRegisteredInStarCloudServices()
        {
            // Specify your AllReceipts config path.
            string filePath = Properties.Settings.Default.AllReceiptsRegistrationFilePath;

            if (filePath.Equals(""))
            {
                return false;
            }

            SMCloudServices.SetRegistrationConfigFilePath(filePath);

            return SMCloudServices.IsRegistered();
        }

        /// <summary>
        /// Sample : Deleting AllReceipts registration saved file. (Using this method when your application is uninstalled)
        /// </summary>
        public static void DeleteRegistrationConfigFile()
        {
            // Specify the registration information saved file path.
            string registratioConfignFilePath = SMCloudServices.GetRegistrationConfigFilePath();

            try
            {
                File.Delete(registratioConfignFilePath);
            }
            catch (Exception)
            {

            }

            bool success;

            if (!File.Exists(registratioConfignFilePath))
            {
                success = true;
            }
            else
            {
                success = false;
            }

            ShowResultMessage(success);

            Util.NotifyAllReceiptsIsRegisteredStatusChanged();
        }

        public static void PrintLocalizeReceiptWithAllReceipts(ReceiptInformationManager receiptInfo)
        {
            CommunicationResult result = new CommunicationResult();

            ProgressBarWindow progressBarWindow = new ProgressBarWindow("Communicating...", () =>
            {
                // Create print receipt with AllReceipts commands.
                byte[] commands = CreateLocalizeReceiptWithAllReceiptsCommands(receiptInfo);

                if (commands == null) // All print settings (Receipt, Information, QR Code) are OFF.
                {
                    result.Result = Communication.Result.Success;
                    result.Code = StarResultCode.Succeeded;

                    return;
                }

                // Your printer PortName and PortSettings.
                string portName = SharedInformationManager.SelectedPortName;
                string portSettings = SharedInformationManager.SelectedPortSettings;

                // Send commands to printer
                result = Communication.SendCommands(commands, portName, portSettings, 30000);
            });

            progressBarWindow.ShowDialog();

            Communication.ShowCommunicationResultMessage(result);
        }

        private static string SetRegistrationConfigFilePath()
        {
            string filePath = ShowSelectXmlFileDialog();

            if (filePath == null)
            {
                return null;
            }

            SharedInformationManager.AllReceiptsRegistrationFilePath = filePath;

            return filePath;
        }

        private static string ShowSelectXmlFileDialog()
        {
            SaveFileDialog saveFIleDialog = new SaveFileDialog();

            string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\StarMicronics";

            SafeCreateDirectory(initialDirectory);

            saveFIleDialog.FileName = "AllReceipts.xml";
            saveFIleDialog.InitialDirectory = initialDirectory;
            saveFIleDialog.Filter = "XML files (*.xml) | *.xml";
            saveFIleDialog.Title = "Select XML file";
            saveFIleDialog.RestoreDirectory = true;
            saveFIleDialog.OverwritePrompt = false;

            if (saveFIleDialog.ShowDialog() == true)
            {
                return saveFIleDialog.FileName;
            }

            return null;
        }

        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }

            return Directory.CreateDirectory(path);
        }

        public static void ShowResultMessage(bool success)
        {
            string message = "";

            if (success)
            {
                message = "Success";
            }
            else
            {
                message = "Failure";
            }

            MessageBox.Show(message, "Result");
        }
    }

    public partial class AllReceiptsSamplePage : Page
    {
        public AllReceiptsSamplePage()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AllReceiptsSampleManager.AddAllReceiptsFunctionEvent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AllReceiptsSampleManager.RemoveAllReceiptsFunctionEvent();
        }
    }

    public class AllReceiptsRegistrationClickEvent : BaseCommand
    {
        public override void Execute(object parameter)
        {
            AllReceiptsSampleManager.ShowAllReceiptsRegistrationView();
        }
    }

    public class PrintLocalizeReceiptWithAllReceiptsClickEvent : BaseCommand
    {
        public ReceiptInformationManager ReceiptInformationManager { get; set; }

        public override void Execute(object parameter)
        {
            AllReceiptsSampleManager.PrintLocalizeReceiptWithAllReceipts(ReceiptInformationManager);
        }
    }

    public class DeleteRegistrationFileClickEvent : BaseCommand
    {
        public override void Execute(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Delete AllReceipts config file?", "Confirm", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                AllReceiptsSampleManager.DeleteRegistrationConfigFile();
            }
        }
    }
}
