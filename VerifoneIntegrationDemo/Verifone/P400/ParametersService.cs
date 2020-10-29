using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verifone.Vim.Api.Common;
using Verifone.Vim.Api.Common.Print;
using Verifone.Vim.Api.Common.TextFormatting;
using Verifone.Vim.Api.Common.Token;
using Verifone.Vim.Api.Parameters;

namespace P400
{
    public static class ParametersService
    {
        public static LoginParameters GetLoginParameters(string ecrId,string ecrSerial)
        {
            try
            {
                return new LoginParameters.Builder()
                    .SoftwareVersion("1.2.4 build 8")
                    .SoftwareName("VerifoneSmartClient")
                    .SoftwareManufacturer("MyCompany inc.")
                    .EcrId(ecrId)
                    .EcrSerial(ecrSerial)
                    .AddEcrCapability(EcrCapabilitiesType.CashierInput)
                    .AddEcrCapability(EcrCapabilitiesType.CashierDisplay)
                    .AddEcrCapability(EcrCapabilitiesType.CashierStatus)
                    .AddEcrCapability(EcrCapabilitiesType.CashierError)
                    .AddEcrCapability(EcrCapabilitiesType.PrinterReceipt)
                    .AddEcrCapability(EcrCapabilitiesType.SignatureCapture)
                    .AddEcrCapability(EcrCapabilitiesType.SignatureVerification)
                    .AddEcrCapability(EcrCapabilitiesType.PrinterDocument)
                    .AddEcrCapability(EcrCapabilitiesType.TerminalReplication)
                    .AddEcrCapability(EcrCapabilitiesType.CustomerInput)
                    .EcrLanguage(LanguageType.English)
                    .Build();

            }
            catch (ArgumentException e)
            {
                // Invalid argument provided
                throw new Exception(e.Message);
            }
        }

        /**
         * ecrTransactionId: Transaction-Id ?????*/
        public static TransactionParameters GetTransactionParameters(string ecrId, string ecrTransactionId, decimal amount, CurrencyType currencyType)
        {
            try
            {
                return new TransactionParameters.PurchaseBuilder()
                    .EcrId(ecrId)
                    .EcrTransactionId(new TransactionId(ecrTransactionId, DateTime.Now))
                    .Amount(amount)
                    .Currency(currencyType)
                    .Build();
            }
            catch (ArgumentException e)
            {
                // Invalid argument provided
                throw new Exception(e.Message);

            }
        }


        public static PrintParameters GetPrintParameters()
        {
            try
            {
                return new PrintParameters.Builder()
                    .EcrId("EcrA")
                    .AddPrintElement("Simple text line")
                    .AddPrintElement(new TextPrintElement.Builder().Text("Formatted text line").TextStyle(TextStyle.Bold).TextAlignment(TextAlignment.Right).TextHeight(TextHeight.DoubleHeight).TextWidth(TextWidth.DoubleWidth).Build())
                    .AddPrintElement(new TextPrintElement.Builder().Text("Partial line ").EndOfLineFlag(false).Build())
                    .AddPrintElement(new TextPrintElement.Builder().Text("Rest line").TextStyle(TextStyle.Bold).Build())
                    .AddPrintElement(new LinearBarcodePrintElement.Builder().Type(BarcodeType.EAN8).Value("02200220").Build())
                    .AddPrintElement("")
                    .AddPrintElement(new LinearBarcodePrintElement.Builder().Type(BarcodeType.Code128).Value("test12345").Height(100).Width(300).Build())
                    .AddPrintElement("")
                    .AddPrintElement(new QrCodePrintElement.Builder().Value("verifone.com").Height(300).Width(300).ErrorCorrection(QrCodeErrorCorrectionType.High).Build())
                    .AddPrintElement("")
                    .AddPrintElement(new AztecCodePrintElement.Builder().Value("verifone.com").Height(300).Width(300).Build())
                    .AddPrintElement("").AddPrintElement("").AddPrintElement("").Build();
            }
            catch (ArgumentException e)
            {
                // Invalid argument provided
                throw new Exception(e.Message);

            }
        }

    }
}