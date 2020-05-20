using StarMicronics.SMCloudServicesSolution;
using StarMicronics.StarIOExtension;
using System;
using System.ComponentModel;

namespace StarPRNTSDK
{
    public class AllReceiptsInfoManager : INotifyPropertyChanged
    {
        public AllReceiptsInfoManager()
        {
            if (SharedInformationManager.AllReceiptsInfoManager == null)
            {
                RestorePreviousSettings();
            }
            else
            {
                CopyProperty(SharedInformationManager.AllReceiptsInfoManager);
            }

            SharedInformationManager.AllReceiptsInfoManager = this;

            string filePath = Properties.Settings.Default.AllReceiptsRegistrationFilePath;

            SMCloudServices.SetRegistrationConfigFilePath(filePath);

            try
            {
                isRegistered = SMCloudServices.IsRegistered();
            }
            catch (InvalidOperationException)
            {
                isRegistered = false;
            }
        }

        private void RestorePreviousSettings()
        {
            PrintReceipt = Properties.Settings.Default.AllReceiptsPrintReceipt;
            PrintInformation = Properties.Settings.Default.AllReceiptsPrintInformation;
            PrintQrCode = Properties.Settings.Default.AllReceiptsPrintQrCode;
        }

        private void CopyProperty(AllReceiptsInfoManager source)
        {
            PrintReceipt = source.PrintReceipt;

            PrintInformation = source.PrintInformation;

            PrintQrCode = source.PrintQrCode;
        }

        public string RegistrationStateDescription
        {
            get
            {
                return CreateRegistrationStateDescription();
            }
        }

        public bool IsRegistered
        {
            get
            {
                return isRegistered;
            }
        }

        public bool IsUnRegistered
        {
            get
            {
                return !isRegistered;
            }
        }

        private bool isRegistered;

        public bool PrintReceipt
        {
            get
            {
                return printReceipt;
            }
            set
            {
                printReceipt = value;

                Properties.Settings.Default.AllReceiptsPrintReceipt = value;

                Properties.Settings.Default.Save();
            }
        }

        private bool printReceipt;

        public bool PrintInformation
        {
            get
            {
                return printInformation;
            }
            set
            {
                printInformation = value;

                Properties.Settings.Default.AllReceiptsPrintInformation = value;

                Properties.Settings.Default.Save();
            }
        }

        private bool printInformation;

        public bool PrintQrCode
        {
            get
            {
                return printQrCode;
            }
            set
            {
                printQrCode = value;

                Properties.Settings.Default.AllReceiptsPrintQrCode = value;

                Properties.Settings.Default.Save();
            }
        }

        private bool printQrCode;

        public bool TextReceiptIsEnabled
        {
            get
            {
                if (IsUnRegistered ||
                    !SharedInformationManager.SelectedModelManager.TextReceiptIsEnabled)
                {
                    return false;
                }

                return true;
            }
        }

        public bool TextUTF8ReceiptIsEnabled
        {
            get
            {
                if (IsUnRegistered ||
                    !SharedInformationManager.SelectedModelManager.TextUTF8ReceiptIsEnabled)
                {
                    return false;
                }

                return true;
            }
        }

        public bool RasterReceiptIsEnabled
        {
            get
            {
                if (IsUnRegistered ||
                    !SharedInformationManager.SelectedModelManager.RasterReceiptIsEnabled)
                {
                    return false;
                }

                return true;
            }
        }

        public void NotifyIsRegisteredPropertyChanged()
        {
            isRegistered = SMCloudServices.IsRegistered();

            OnPropertyChanged("RegistrationStateDescription");
            OnPropertyChanged("IsRegistered");
            OnPropertyChanged("IsUnRegistered");
            OnPropertyChanged("TextReceiptIsEnabled");
            OnPropertyChanged("TextUTF8ReceiptIsEnabled");
            OnPropertyChanged("RasterReceiptIsEnabled");
        }

        private string CreateRegistrationStateDescription()
        {
            string description = "";

            if (IsRegistered)
            {
                description = "Registration Details";
            }
            else
            {
                description = "Unregistered Device";
            }

            return description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
