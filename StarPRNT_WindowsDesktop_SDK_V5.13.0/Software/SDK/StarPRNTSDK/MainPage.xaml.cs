using StarMicronics.SMCloudServicesSolution;
using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StarPRNTSDK
{
    /// <summary>
    /// Sample : Getting library version.
    /// </summary>
    public static class LibraryVersionSampleManager
    {
        // StarIO
        public static string GetStarIOVersion()
        {
            return Factory.I.GetStarIOVersion();
        }

        // StarIOExtension
        public static string GetStarIOExtVersion()
        {
            return StarIoExt.GetStarIOExtVersion();
        }

        // StarCloudServices
        public static string GetSMCloudServicesVersion()
        {
            return SMCloudServices.GetSMCloudServicesVersion();
        }

        public static void ShowLibraryVersion()
        {
            string starIOVersion = GetStarIOVersion();
            string starIOExtVersion = GetStarIOExtVersion();
            string smCoudServicesVersion = GetSMCloudServicesVersion();

            string message = "StarIO : version " + starIOVersion + "\n" +
                             "StarIOExtension : version " + starIOExtVersion + "\n" +
                             "SMCloudServices : version " + smCoudServicesVersion;

            MessageBox.Show(message, "Library Version");
        }
    }

    /// <summary>
    /// Sample : Getting printer serial number.
    /// </summary>
    public static class SerialNumberSampleManager
    {
        public static void ShowPrinterSerialNumber()
        {
            // Your printer PortName and PortSettings.
            string portName = SharedInformationManager.SelectedPortName;
            string portSettings = SharedInformationManager.SelectedPortSettings;

            string serialNumber = "";

            // Getting printer serial number sample is in "Communication.GetProductSerialNumber(ref string serialNumber, IPort port)".
            CommunicationResult result = Communication.GetProductSerialNumberWithProgressBar(ref serialNumber, portName, portSettings, 30000);

            if (result.Result == Communication.Result.Success)
            {
                MessageBox.Show(serialNumber, "Product Serial Number");
            }
            else
            {
                Communication.ShowCommunicationResultMessage(result);
            }
        }
    }


    public partial class MainPage : Page
    {
        public Uri JumpUri { get; set; }

        public MainPage()
        {
            InitializeComponent();

            JumpUri = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveAllJournals();

            Util.GoNextPage(JumpUri);

            JumpUri = null;
        }

        private void RemoveAllJournals()
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }
    }

    public class ShowSerialNumberClickEvent : BaseCommand
    {
        public override void Execute(object parameter)
        {
            SerialNumberSampleManager.ShowPrinterSerialNumber();
        }
    }

    public class ShowLibraryVersionClickEvent : BaseCommand
    {
        public override void Execute(object parameter)
        {
            LibraryVersionSampleManager.ShowLibraryVersion();
        }
    }
}
