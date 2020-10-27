using NLog;
using System;
using System.IO;
using System.Net;
using Verifone.Vim.Api;
using Verifone.Vim.Api.Common;
using Verifone.Vim.Api.Configuration;
using Verifone.Vim.Api.Events;
using Verifone.Vim.Api.Listeners;
using Verifone.Vim.Api.Results;
using Verifone.Vim.Api.TerminalInformation;
using Verifone.Vim.Api.TerminalInformation.TerminalConnection;

namespace P400
{
    public class VimManager : ITerminalConnectListener, IVimListener, IVimApiListener
    {
        IVim vim;
        IVimApi terminalApi;
        Logger log = LogManager.GetCurrentClassLogger();
        public VimManager()
        {
            InitialiseVim();
        }
        public void InitialiseVim()
        {
            // Create VIM instance
            try
            {
                vim = VimFactory.CreateVim(new VimConfig.Builder()
                 //.ConnectMode(VimConnectMode.EcrConnectMode)
                 .LogLocation(Directory.GetCurrentDirectory())
                 .LogLevel(VimLogLevel.DEBUG)
                 .DefaultCurrency(CurrencyType.SEK)
                 .Build());
            }
            catch (VimConfigException e)
            {
                // Unable to configure VIM
                log.Error($"Unable to configure VIM {e.Message}");
            }
            // Add optional TerminalConnectListener to accept/reject connecting payment terminals
            vim.SetTerminalConnectListener(this);
            // Add mandatory VimListener
            vim.AddVimListener(this);

            // Initialise VIM and start listening for payment terminals
            try
            {
                vim.Initialise();
            }
            catch (InvalidOperationException e)
            {
                // Initialization failed
                log.Error($"Initialization failed {e.Message}");
            }


            //Set terminal MixedConnectionMode
            //InitialisedMixedConnectionMode();

        }

        private void InitialisedMixedConnectionMode()
        {
            try
            {
                // Construct TerminalInformation instance
                TerminalInformation terminalInformation = new TerminalInformation.Builder()
                .SerialNumber("346-742-881")
                .TerminalConnection(new TcpTerminalConnection.Builder()
                .SocketAddress(new IPEndPoint(IPAddress.Parse("172.31.79.202"), 9000))
                .ConnectionInitiationType(ConnectionInitiationType.EcrInitiated)
                .ConnectionManagementType(ConnectionManagementType.ConnectionPerSession
               )
                .Build())
                .Build();
                // Create terminal
                vim.CreateTerminal(terminalInformation);
            }
            catch (InvalidOperationException e)
            {
                // Invalid VIM state
                log.Error($"Invalid VIM state {e.Message}");
            }
            catch (ArgumentException e)
            {
                // Invalid parameters provided
                log.Error($"Invalid parameters provided {e.Message}");
            }
        }

        public void OnAccountSearchEvent(AccountSearchEvent accountSearchEvent)
        {
            Console.WriteLine($"VimInitialisation OnAccountSearchEvent TerminalId: {accountSearchEvent.TerminalId}");
        }

        public void OnBarcodeScanEvent(BarcodeScanEvent barcodeScanEvent)
        {
            Console.WriteLine($"VimInitialisation OnBarcodeScanEvent TerminalId: {barcodeScanEvent.TerminalId}");
        }

        public void OnCardEvent(CardEvent cardEvent)
        {
            Console.WriteLine($"OnCardEvent TerminalId: {cardEvent.TerminalId} ");
        }

        public void OnCommunicationErrorEvent(CommunicationErrorEvent communicationErrorEvent)
        {
            Console.WriteLine("OnCommunicationErrorEvent");
        }

        public void OnConnectionChangedEvent(ConnectionChangedEvent connectionChangedEvent)
        {
            Console.WriteLine("OnConnectionChangedEvent...");
            var loginManager = new LoginManager(TerminalApi.TerminalApiInstance);
            loginManager.Login();
        }

        public void OnTerminalConnect(TerminalConnectEvent connectEvent)
        {
            Console.WriteLine("OnTerminalConnect...");

            // Get payment terminal information
            // Accept payment terminal connecting to VIM
            connectEvent.AcceptTerminal();
        }
        public void OnTerminalMaintenanceEvent(MaintenanceEvent maintenanceEvent)
        {
            Console.WriteLine("OnTerminalMaintenanceEvent");
        }

        public void OnTerminalReady(TerminalReadyEvent readyEvent)
        {

            // Get VimApi instance for interacting with the payment terminal
            Terminal terminal = readyEvent.Terminal;
            TerminalApi.TerminalApiInstance = terminal.GetVimApi(this);


            var loginManager = new LoginManager(TerminalApi.TerminalApiInstance);
            loginManager.Login();

        }

        public void OnVimError(VimErrorEvent evnt)
        {
            Console.WriteLine($"VimInitialisation OnVimError TerminalId: {evnt.ErrorMessage}");
        }
    }
}