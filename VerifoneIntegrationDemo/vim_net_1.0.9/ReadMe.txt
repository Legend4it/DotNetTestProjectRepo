|--------------------------------|
| VIM.net Package Contents Guide |
|--------------------------------|
|  Verifone, September 2nd 2020  |
|--------------------------------|


CONTENTS
--------
1. TERMS OF USE
2. PACKAGE CONTENTS
3. INSTALLATION
4. CHANGELOG

1. TERMS OF USE
---------------
A user agreement with your local Verifone office for VIM must be signed before the contents of this package can be used. 
The contents of this package may not be distributed to third parties.


2. PACKAGE CONTENTS
-------------------
- Verifone.Vim.1.0.9.nupkg                  : The VIM library for .net (NuGet package with support for .NET Standard 1.5 & 2.0, .NET Framework 3.5, 4.0, 4.5 or later and .NET Compact Framework 3.5)
- VIM_Programmers_Guide_dotNet_20200902.pdf : VIM Programmers' guide


3. INSTALLATION
---------------
- See https://www.nuget.org for general information about NuGet packages.
- The simplest way of making the VIM NuGet package available to a project is to setup a local 
  NuGet feed as described here: https://docs.microsoft.com/en-us/nuget/hosting-packages/overview

4. CHANGELOG
------------
Changes in version 1.0.9:
- Fixed problem making it possible to start two transactions against the same terminal simultaneously when using multiple threads.
- Minor internal improvements.

Changes in version 1.0.8:
- Support for passing gateway login credentials. 
  (Only relevant in certain countries like the UK).
- Support for passing proxy software version and update info. 
  (Only relevant in special setups when using a proxy software between terminal and ECR software).
- Support for long running Login dialogs.
  (Only relevant in certain countries like the UK).
- Added AcquirerMerchantId and AcquirerTerminalId to TransactionResult when available.
- Improved checking of input received from the ECR software in connecting with OnInputRequest callbacks.
- Internal fixes and improvements.

Changes in version 1.0.7:
- Support for non-incremental reservations, i.e. support for creating, completing and cancelling reservations.
- Internal fixes and improvements.

Changes in version 1.0.6:
- Support for providing token lookup result info to ECR.
- Fixed problem with terminal reconnect after VIM reinitialisation (unInitialise() followed by initialise()).
- Internal fixes and improvements.

Changes in version 1.0.5:
- Support for pay at table
- Internal fixes and improvements.
- API changes:
    * Added new method 'onAccountSearchEvent(AccountSearchEvent)' to VimApiListener interface.

Changes in version 1.0.4:
- Support for printing on terminal.
- Support for allowing mobile terminals to enter sleep mode.
- Support for balance inquiry.
- Support for alternative payments (i.e. transactions where the PaymentInstrumentType=AlternativePayments).
- Support for token based enrolment.
- Support for specifying token brand priority.
- Internal fixes and improvements.

Changes in version 1.0.3:
- Fixed VIM logging configuration to avoid replacing existing NLog configuration.
- Minor internal fixes and improvements.

Changes in version 1.0.2:
- Fixed uninitialisation bug related to terminal reconnects.

Changes in version 1.0.1:
- Support for connecting to terminals.
- Support for printing other document types than receipts.
- Support added for .NET Framework 3.5, 4.0 and .NET Compact Framework 3.5
- Internal fixes and improvements.

Changes in version 1.0.0:
- Fixed uninitialisation & crash bugs
- Functionality now on par with VIM Java 1.0.11
- API changes:
    * The method PrintEnded() in Verifone.Vim.Api.DeviceRequests.Print has been replaced by PrintSuccess() and a PrintFailure() has also been added.
    * Changed/added Verifone.Vim.Api.Common.ServiceIdentificationType values used to start Admin transactions:
        - AcquirerConnectionTest renamed to GatewayConnectionTest.
        - SendOfflineTransactions added to avoid future problems and confusion around use of AcquirerReconciliation. 
          Please use SendOfflineTransactions for sending offline stored transactions to host from now on. 
          AcquirerReconciliation should only be used if the intension is to initiate a reconciliation.
    * Renamed Verifone.Vim.Api.Common.Receipt property RequiredSignature to SignatureRequired for consistency and readability.
    * Fixed typos in Verifone.Vim.Api.DeviceRequests.Input.InputRequestData property names:
        - DefaultInputstring renamed to DefaultInputString
        - EdrId renamed to EcrId
        
