using POPScanner;
using StarMicronics.StarIO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Text;
using FortusPOSClient.Model;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;

namespace FortusPOSClient
{
    public partial class MainWindow : Window
    {
        private IPort port;
        private string tGuid;
        private string tName;

        public MainWindow()
        {
            InitializeComponent();
            tGuid = Guid.NewGuid().ToString();
            txtPrinterGuid.Content = tGuid;
            InitViewAsync();
        }

        public async Task InitViewAsync()
        {


            foreach (PortInfo item in Factory.I.SearchPrinter(PrinterInterfaceType.USBPrinterClass))
            {
                var portSetting = item.PortName.Split(':')[0];
                port = Factory.I.GetPort(item.PortName, portSetting/*"USBPRN"*/, 1000);
                tName = item.ModelName;
                txtInfo.Text = $"Version:{Factory.I.GetStarIOVersion()}{Environment.NewLine}ModelName:{item.ModelName}{Environment.NewLine}PortName:{item.PortName}{Environment.NewLine}PortSetting:{portSetting}{Environment.NewLine}USBSerialNumber:{item.USBSerialNumber}{Environment.NewLine}MacAddress:{item.MacAddress}{Environment.NewLine}HashCode:{Factory.I.GetHashCode()}";
            }

            var dueTime = TimeSpan.FromSeconds(1);
            var interval = TimeSpan.FromSeconds(2);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            await RunPeriodicAsync(BarcodeReader, dueTime, interval, CancellationToken.None);



        }

        // The `onTick` method will be called periodically unless cancelled.
        private async Task RunPeriodicAsync(Action onTick,
                                                   TimeSpan dueTime,
                                                   TimeSpan interval,
                                                   CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                onTick?.Invoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void BarcodeReader()
        {
            var bcrCode = BarCodeFactory.MonitoringBarcodeReader(port).Trim();
            // get the user input for every iteration, allowing to exit at will
            lblDcrCode.Content = bcrCode;
            BarCodeFactory.ClearBarcodeReaderBuffer(port);
            if (!string.IsNullOrWhiteSpace(bcrCode))
            {
                PostBcrCodeToApi(new RequestContent { BcrCode = bcrCode, TerminalGuid = tGuid, TerminalName = tName, RelPartnerId = 11 }); //Test partner id, need to identify artikel by partner and Barcode 
            }
        }


        private void PostBcrCodeToApi(RequestContent rContent)
        {

            HttpClient client = new HttpClient();
            string apiUrl = ConfigurationManager.AppSettings["BcrCodeApi"] + "/bcrcode";
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = JsonConvert.SerializeObject(rContent);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                //Test Print to mPOP 10
                //PrintTestBcrCode(rContent);
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "Exception", ex.Message.ToString());
                //throw new Exception(ex.Message);
            }
        }

        private void PrintTestBcrCode(RequestContent rContent)
        {
            var font = new Font("Arial", 10);
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = rContent.TerminalName;
            pd.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(rContent.BcrCode, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));

            };
            pd.Print();
        }
    }
}
