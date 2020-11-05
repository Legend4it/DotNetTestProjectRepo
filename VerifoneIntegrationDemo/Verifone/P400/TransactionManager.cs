using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verifone.Vim.Api;
using Verifone.Vim.Api.Common;
using Verifone.Vim.Api.Common.Content;
using Verifone.Vim.Api.Common.PaymentInstrumentData;
using Verifone.Vim.Api.Common.Receipt;
using Verifone.Vim.Api.Common.Token;
using Verifone.Vim.Api.DeviceRequests.Display;
using Verifone.Vim.Api.DeviceRequests.Input;
using Verifone.Vim.Api.DeviceRequests.Print;
using Verifone.Vim.Api.Listeners;
using Verifone.Vim.Api.Parameters;
using Verifone.Vim.Api.Results;

namespace P400
{
    public class TransactionManager : ITransactionResultListener
    {
        IVimApi _vimApi;
        TransactionParameters parameter;
        Logger log = LogManager.GetCurrentClassLogger();
        PrintManager printManager;

        public TransactionManager(IVimApi vimApiInstance)
        {
            _vimApi = vimApiInstance;
            printManager = new PrintManager();
        }
        public void StartTransaction()
        {
            parameter = ParametersService.GetTransactionParameters("EcrA", "12345",100,CurrencyType.SEK);
            _vimApi.StartTransaction(parameter, this);
        }

        public void OnDisplayRequest(DisplayRequestData displayRequestData)
        {
            // Get information about the display message received from the terminal
            List<DisplayOutput> displayMessages = displayRequestData.DisplayOutput;
            foreach (DisplayOutput displayMessage in displayMessages)
            {
                DeviceType displayMessageType = displayMessage.DeviceType;
                DisplayContent displayMessageContent = displayMessage.Content;
                if (displayMessageContent.Format == DisplayFormatType.Text)
                {
                    ContentText contentText = displayMessageContent.Text;
                    string plainTextDisplayMessage = contentText.PlainText;
                }
            }
        }

        public void OnFailure(TransactionFailureResult result)
        {
            // Get information about the rejected transaction
            TransactionType transType = result.TransactionType;
            TransactionId ecrTransId = result.EcrTransactionId;
            TransactionId terminalTransId = result.TerminalTransactionId;
            FailureErrorType errorType = result.Error;
            string additionalReason = result.AdditionalReason;
            List<Receipt> receipts = result.Receipts;
            foreach (Receipt receipt in receipts)
            {
                DocumentType documentType = receipt.DocumentType;
                bool? signatureRequired = receipt.SignatureRequired;
                ReceiptContent receiptContent = receipt.Content;
                if (receiptContent.Format == ReceiptFormatType.Text)
                {
                    ContentText contentText = receiptContent.Text;
                    string plainTextReceipt = contentText.PlainText;
                }
            }
            List<Token> tokens = result.Tokens;
            foreach (Token token in tokens)
            {
                TokenType tokenType = token.Type;
                string tokenValue = token.Value;
                DateTime? tokenExpiry = token.Expiry;
                string tokenSchemeId = token.SchemeId;
                EnrolmentStatusType? enrolmentStatusType = token.EnrolmentStatus;
            }
            // TokenResponses give info about the result of token lookups and
            // provides an alternative way to get the token when the lookup was successful.
            List<TokenResponse> tokenResponses = result.TokenResponses;
            foreach (TokenResponse tokenResponse in tokenResponses)
            {
                TokenRequest tokenRequest = tokenResponse.TokenRequest;
                string tokenSchemeId = tokenRequest.SchemeId;
                if (tokenResponse.Result == TokenResultType.Success)
                {
                    Token token = tokenResponse.Token;
                }
                else
                {
                    TokenFailureErrorType? tokenErrorType = tokenResponse.Error;
                    string tokenErrorDescription = tokenResponse.AdditionalReason;
                }
            }
        }
        public void OnInputRequest(InputRequestData requestData, IInputReceiver inputReceiver)
        {
            Console.WriteLine($"TransactionListener OnInputRequest InputRequestData: {requestData} , IInputReceiver: {inputReceiver}");
            // Get information about the request for input from the terminal
            InputRequestType inputType = requestData.InputType;
            DeviceType inputDevice = requestData.DeviceType;
            string inputDefault = requestData.DefaultInputString;
            int? inputTimeout = requestData.TimeoutInSeconds;
            int? inputMinLength = requestData.MinLength;
            int? inputMaxLength = requestData.MaxLength;
            DisplayOutput output = requestData.DisplayOutput;
            DeviceType outputDevice = output.DeviceType;
            DisplayContent outputContent = output.Content;
            if (outputContent.Format == DisplayFormatType.Text)
            {
                ContentText contentText = outputContent.Text;
                string plainTextDisplayMessage = contentText.PlainText;
            }
            // Return user input
            inputReceiver.InputText("1234"); //see chapter "Input requests" for details
        }
        public void OnInputRequestAborted(InputRequestAbortedData inputRequestAbortedData)
        {
            Console.WriteLine($"TransactionListener OnInputRequestAborted InputRequestAbortedData: {inputRequestAbortedData}");
            // InputRequest aborted
        }
        public void OnPrintRequest(PrintRequestData requestData, IPrintReceiver printReceiver)
        {
            // Get information about the requested print received from the terminal
            DocumentType documentType = requestData.DocumentType;
            PrintContent printContent = requestData.Content;
            if (printContent.Format == PrintFormatType.Text)
            {
                ContentText contentText = printContent.Text;
                string plainTextPrint = contentText.PlainText;
            }
            // Handle print
            // Return response
            printReceiver.PrintSuccess();
        }

        public void OnSuccess(TransactionResult result)
        {
            // Get information about the approved transaction
            TransactionType transType = result.TransactionType;
            decimal transAmount = result.AuthorizedAmount;
            decimal cashbackAmount = result.CashbackAmount;
            decimal tipAmount = result.TipAmount;
            TransactionId ecrTransId = result.EcrTransactionId;
            TransactionId terminalTransId = result.TerminalTransactionId;
            string approvalCode = result.ApprovalCode;
            PaymentInstrumentData instrumentData = result.PaymentInstrumentData;
            PaymentInstrumentType? instrumentType = instrumentData?.PaymentInstrumentType;
            if (instrumentType == PaymentInstrumentType.Card)
            {
                CardData cardData = instrumentData.CardData;
                EntryModeType entryMode = cardData.EntryMode;
                string paymentBrand = cardData.PaymentBrand;
                string maskedPan = cardData.MaskedPan;
            }
            else if (instrumentType == PaymentInstrumentType.AlternativePayment)
            {
                AlternativePaymentData alternativePaymentData = instrumentData.AlternativePaymentData;
                string alternativePaymentBrand = alternativePaymentData.AlternativePaymentBrand;
                EntryModeType entryMode = alternativePaymentData.EntryMode;
                TransactionId providerTransactionId = alternativePaymentData.ProviderTransactionId;
            }
            List<Receipt> receipts = result.Receipts;
            foreach (Receipt receipt in receipts)
            {
                DocumentType documentType = receipt.DocumentType;
                bool? signatureRequired = receipt.SignatureRequired;
                ReceiptContent receiptContent = receipt.Content;
                if (receiptContent.Format == ReceiptFormatType.Text)
                {
                    ContentText contentText = receiptContent.Text;
                    string plainTextReceipt = contentText.PlainText;
                }
            }
            List<Token> tokens = result.Tokens;
            foreach (Token token in tokens)
            {
                TokenType tokenType = token.Type;
                string tokenValue = token.Value;
                DateTime? tokenExpiry = token.Expiry;
                string tokenSchemeId = token.SchemeId;
                EnrolmentStatusType? enrolmentStatusType = token.EnrolmentStatus;
            }
            // TokenResponses give info about the result of token lookups and
            // provides an alternative way to get the token when the lookup was successful.
            List<TokenResponse> tokenResponses = result.TokenResponses;
            foreach (TokenResponse tokenResponse in tokenResponses)
            {
                TokenRequest tokenRequest = tokenResponse.TokenRequest;
                string tokenSchemeId = tokenRequest.SchemeId;
                if (tokenResponse.Result == TokenResultType.Success)
                {
                    Token token = tokenResponse.Token;
                }
                else
                {
                    TokenFailureErrorType? tokenErrorType = tokenResponse.Error;
                    string tokenErrorDescription = tokenResponse.AdditionalReason;
                }
            }
        }

        public void OnTimeout(TimeoutReason reason)
        {
            Console.WriteLine($"TransactionListener OnTimeout TimeoutReason: {reason}");
        }
    }
}
