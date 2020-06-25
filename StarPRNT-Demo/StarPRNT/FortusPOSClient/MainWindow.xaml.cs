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

namespace FortusPOSClient
{
    public partial class MainWindow : Window
    {
        private IPort port;

        public MainWindow()
        {
            InitializeComponent();
            InitViewAsync();
        }

        public async Task InitViewAsync()
        {


            foreach (PortInfo item in Factory.I.SearchPrinter(PrinterInterfaceType.USBPrinterClass))
            {
                port = Factory.I.GetPort(item.PortName, "USBPRN", 1000);
                
                txtInfo.Text = $"Version:{Factory.I.GetStarIOVersion()},{Environment.NewLine}ModelName:{item.ModelName},{Environment.NewLine}PortName:{item.PortName},{Environment.NewLine}USBSerialNumber:{item.USBSerialNumber},{Environment.NewLine}MacAddress:{item.MacAddress},{Environment.NewLine}HashCode:{Factory.I.GetHashCode()}";
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
            var bcrCode= BarCodeFactory.MonitoringBarcodeReader(port).Trim();
            // get the user input for every iteration, allowing to exit at will
            lblDcrCode.Content = bcrCode;
            BarCodeFactory.ClearBarcodeReaderBuffer(port);
            if (!string.IsNullOrWhiteSpace(bcrCode))
            {
                PostBcrCodeToApi(bcrCode);
            }
        }


        private void PostBcrCodeToApi(string bcrcode)
        {

            HttpClient client = new HttpClient();
            string apiUrl = ConfigurationManager.AppSettings["BcrCodeApi"] + "/bcrcode";
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(bcrcode, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(apiUrl + "/?code=", content).Result;

                //if (response.IsSuccessStatusCode)
                //{
                //    MessageBox.Show("Send Successfully");
                //}
                //else
                //{
                //    MessageBox.Show("Send Failed...");
                //}

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
