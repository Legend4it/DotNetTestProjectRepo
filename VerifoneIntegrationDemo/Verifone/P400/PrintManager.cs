using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verifone.Vim.Api;
using Verifone.Vim.Api.Common;
using Verifone.Vim.Api.Listeners;
using Verifone.Vim.Api.Parameters;
using Verifone.Vim.Api.Results;

namespace P400
{
    public class PrintManager : IPrintResultListener
    {

        IVimApi _vimApi;
        PrintParameters parameter;
        Logger log = LogManager.GetCurrentClassLogger();

        public PrintManager(IVimApi vimApiInstance)
        {
            _vimApi = vimApiInstance;
        }


        public void StartTerminalPrint()
        {
            parameter = ParametersService.GetPrintParameters();
            _vimApi.StartTerminalPrint(parameter, this);
        }
        public void OnFailure(PrintFailureResult result)
        {
            log.Info($"OnFailure: {result}");
        }

        public void OnSuccess(PrintResult result)
        {
            log.Info($"OnSuccess: {result}");
        }

        public void OnTimeout(TimeoutReason reason)
        {
            log.Info($"OnTimeout: {reason}");
        }
    }
}
