using NLog;
using System;
using System.Collections.Generic;
using Verifone.Vim.Api;
using Verifone.Vim.Api.Common;
using Verifone.Vim.Api.Listeners;
using Verifone.Vim.Api.Parameters;
using Verifone.Vim.Api.Results;

namespace P400
{
    public class LoginManager : ILoginResultListener
    {
        IVimApi _vimApi;
        LoginParameters parameter;
        Logger log = LogManager.GetCurrentClassLogger();

        public LoginManager(IVimApi vimApiInstance)
        {
            _vimApi = vimApiInstance;
        }
        public void Login()
        {
            parameter = ParametersService.GetLoginParameters("EcrA", "123-456-789");
            _vimApi.Login(parameter, this);
        }
        public void OnFailure(LoginFailureResult result)
        {
            // Get information about the failed login
            FailureErrorType errorType = result.Error;
            string additionalReason = result.AdditionalReason;
        }

        public void OnSuccess(LoginResult result)
        {
            // Get information about the successful login
            string terminalSoftwareManufacturer = result.TerminalSoftwareManufacturer;
            string terminalSoftwareName = result.TerminalSoftwareName;
            string terminalSoftwareVersion = result.TerminalSoftwareVersion;
            string terminalSerialNumber = result.TerminalSerialNumber;
            List<TerminalCapabilitiesType> terminalCapabilities = result.TerminalCapabilities;
            TerminalGlobalStatusType terminalGlobalStatus = result.TerminalGlobalStatus;


            var transactionManager = new TransactionManager(TerminalApi.TerminalApiInstance);
            //transactionManager.StartTransaction();
            transactionManager.StartRefund();


        }

        public void OnTimeout(TimeoutReason reason)
        {
            Console.WriteLine($"OnTimeout Result: {reason}");
        }
    }
}
