************************************************************
      StarPRNT SDK Ver 5.13.0
         Readme_En.txt             Star Micronics Co., Ltd.
************************************************************

    1. Overview
    2. Ver5.13.0 Changes
    3. Contents
    4. Scope
    5. Notes
    6. Copyright
    7. Release History

===========================
 1. Overview
===========================

    This package contains StarPRNT SDK Ver 5.13.0.
    StarIOPort/StarIO/StarIOExtension/SMCloudServicesSolution/GenerateBarcode is a library
    for supporting application development for Star printers.

    Refer to SDK documents included in this package for details.

===========================
 2. Ver5.13.0 Changes
===========================

    [SDK]
        - Changed project structure
            * Nuget support for each library.

    [Manual]
        - Changed StarPRNT SDK User's Manual from PDF to online manual link

===========================
 3. Contents
===========================

    StarPRNT_WindowsDesktop_SDK_V5_13_0
    |- Readme_En.txt                                // Release Notes (English)
    |- Readme_Jp.txt                                // Release Notes (Japanese)
    |- SoftwareLicenseAgreement_En.pdf              // Software License Agreement (English)
    |- SoftwareLicenseAgreement_Jp.pdf              // Software License Agreement (Japanese)
    |
    +- Documents
    |  |- UsersManual.url                           // Link to StarPRNT SDK document
    |  |- CommandEmulator_on_SMCS_En.pdf            // Supported command list of Command Emulator on SMCS (English)
    |  +- CommandEmulator_on_SMCS_Jp.pdf            // Supported command list of Command Emulator on SMCS (Japanese)
    |
    +- Software
    |  |- SDK                                       // Samples (Ver 5.13.0)
    |  |- Distributables                            // Not strong named libraries
    |  |   |- StarIOPort_x86.dll                    // StarIOPort_x86.dll          (Ver 2.6.0 for 32 bit application)
    |  |   |- StarIOPort_x64.dll                    // StarIOPort_x64.dll          (Ver 2.6.0 for 64 bit application)
    |  |   |- GenerateBarcode_x86.dll               // GenerateBarcode_x86.dll     (Ver 1.0.0 for 32 bit application)
    |  |   |- GenerateBarcode_x64.dll               // GenerateBarcode_x64.dll     (Ver 1.0.0 for 64 bit application)
    |  |   |- StarIO.dll                            // StarIO.dll                  (Ver 2.6.0)
    |  |   |- StarIOExtension.dll                   // StarIOExtension.dll         (Ver 1.6.0)
    |  |   +- SMCloudServicesSolution.dll           // SMCloudServicesSolution.dll (Ver 1.1.1)
    |  |
    |  +- Distributables_strong_named               // Strong named libraries
    |      |- StarIOPort_x86.dll                    // StarIOPort_x86.dll          (Ver 2.6.0 for 32 bit application)
    |      |- StarIOPort_x64.dll                    // StarIOPort_x64.dll          (Ver 2.6.0 for 64 bit application)
    |      |- GenerateBarcode_x86.dll               // GenerateBarcode_x86.dll     (Ver 1.0.0 for 32 bit application)
    |      |- GenerateBarcode_x64.dll               // GenerateBarcode_x64.dll     (Ver 1.0.0 for 64 bit application)
    |      |- StarIO.dll                            // StarIO.dll                  (Ver 2.6.0)
    |      |- StarIOExtension.dll                   // StarIOExtension.dll         (Ver 1.6.0)
    |      +- SMCloudServicesSolution.dll           // SMCloudServicesSolution.dll (Ver 1.1.1)
    |
    +- Others
        |- PrinterSoftwareRecoveryTool              // Tools for updating software for driver package (Ver 1.0.0)
        +- StarSoundConverter                       // Tools for converting sound format for melody speaker (Ver 1.0.0)

===========================
 4. Scope
===========================

    Works with these Emulation:
        - StarPRNT Mode
        - Star Line Mode
        - Star Graphic Mode
        - ESC/POS
        - ESC/POS Mobile
        - Star Dot Impact

    Please refer to each command specification for details.
    You can download this manual from Star web site.

===========================
 5. Notes
===========================

    1. Please note the following points when replacing 'StarIOPort.dll' or 'StarIO.dll' included in SDK for Windows Desktop UI(Ver2.2 or earlier)
        to 'StarIOPort.dll' or 'StarIO.dll' included in this package.

        - If the following environment is not satisfied, can not build the application.
            OS                       : Windows7, 8/8.1, 10
            Visual Studio            : Visual Studio 2008 or later
            Target framework         : .Net Framework 3.5 or later

        - If using SDK for mobile printer, need to change the arguments of 'GetPort' method.
            Please refer to StarPRNT SDK Document included in this package and
            reset the arguments of 'GetPort' method.
            After that, please check the operation of application.

    2. If mC-Sound was connected after the printer power was turned ON, melody speaker API does not work properly.
        Please turn on the printer after connecting mC-Sound to it.

    3. Change the behavior of ReleasePort method
        - Beginning from StarIO.dll V2.6.0 (StarPRNT SDK V5.11.0), the ReleasePort method behavior has been changed as shown below.
            V2.5.0 and before:
                In the case of calling GetPort method multiple times before calling ReleasePort method,
                the port is released by calling ReleasePort method the same number of times as the GetPort method was called.
            V2.6.0 and later:
                The port is always released when ReleasePort method is called.

===========================
 6. Copyright
===========================

    Copyright 2019 Star Micronics Co., Ltd. All rights reserved.

===========================
 7. Release Notes
===========================

    Ver. 5.13.0
    12/25/2019
        [SDK]
            - Changed project structure
                * Nuget support for each library.

        [Manual]
            - Changed StarPRNT SDK User's Manual from PDF to online manual link

    Ver. 5.11.0
    09/04/2019
        [StarIO]
        - Added features
            * Added the ErrorCode property to PortException class.
        - Changed specification
            * Change the behavior of ReleasePort method
            Before:
                In the case of calling GetPort method multiple times before calling ReleasePort method,
                the port is released by calling ReleasePort method the same number of times as the GetPort method was called.
            After:
                The port is always released when ReleasePort method is called.

        [StarIOExtension]
            -  Added features
                * Changed to enable/disable black mark setting of StarPRNT emulation mobile printer with AppendBlackMark method.

        [SMCloudServicesSolution]
            - Fixed Bugs
                * Fixed layout of view displayed by ShowRegistrationView method.

        [SDK]
            - Added Sample Codes
                * Added sample codes for getting the cause of communication failure with the printer.

    Ver. 5.10.0
    03/07/2019
        [StarIO]
            - Added feature
                * Added a feature that specifies BD address as portName parameter of GetPort method.

        [StarIOExtension]
        - Added features
            * Added AppendCjkUnifiedIdeographFont method.
            It allows you to specify the priority of CJK integrated Kanji font in UTF-8.
            Supported printer: TSP650II(JP2/TW) with firmware version 4.0 or later, mC-Print2 and mC-Print3
            * Added ExtMode which specifies the barcode mode (symbol width) conforming to
            each command specification to BarcodeWidth enumeration.
        - Fixed Bugs
            * Fixed a bug where the image to be printed was moved twice the absolute position of
            the position specified by the AppendBitmapWithAbsolutePosition method in ESC/POS emulation.
            * Fixed a bug that the print start position on the horizontal axis is not set
            to the head of the line even if 0 is specified for the position in the
            AppendBitmapWithAbsolutePosition method in page mode.
            * Fixed a bug that blank spaces at the bottom of bitmap entered by
            AppendBitmap method are not printed when StarPRNT emulation is specified.

        [SDK]
        - Changed sample code
            * Changed the implementation of printing the sample of CJK unified ideographs
            in UTF-8 to use the AppendCjkUnifiedIdeographFont method.

    Ver. 5.9.0
    11/20/2018
        [StarIOExtension]
            - Added feature
                * Added API for Melody Speaker control.

        [SDK]
        - Added sample codes for Melody Speaker control.

    Ver. 5.7.0
    06/29/2018
        [StarIO]
            - Fixed Bugs
                * Fixed a bug that detecting Ethernet printer fails by SearchPrinter method in specific environment.

        [StarIOExtension]
        - End of support
            * StarIoExt.CreateScaleCommandBuilder Method
            * StarIoExt.CreateScaleConnectParser Method
            * StarIoExt.CreateScaleWeightParser Method
            * ScaleModel Constants
            * IScaleCommandBuilder Interface
            * IPeripheralCommandParser.ReceiveCommands Property
            * IScaleWeightParser Interface

        [SMCloudServices]
        - Added features
            * SMCloudServices.ShowRegistrationWindow Method
            When the password of SCS dash board is changed, you can update local password too.

        [SDK]
        - Added sample codes for Print Re-Direction.

    Ver. 5.6.0
    05/21/2018
        [StarIO]
            - Added features
                * Added mC-Print2 and mC-Print3.
                * Added options to portSettings of GetPort method.
                * Added the ConnectedInterface property to StarPrinterStatus class.
            - Fixed Bugs
                * Fixed a bug that detecting Bluetooth printer fails by SearchPrinter method in specific environment.
                * Fixed a bug that getting USB serial number fails by SearchPrinter method.
            - Added strong named library.

        [StarIOExtension]
        - Added features
            * Added method for setting top margin.
            * Added method for setting printable area.
            * Added drive time parameter and delay time parameter to the AppendSound method.
        - Fixed Bugs
            * Fixed a bug that Parse method of IScaleWeightParser interface might not return proper result.
            * Fixed a bug that Stop method of StarPrintPortJomMonitor class might not finish properly.
        - Added strong named library.

        [SMCloudServicesSolution]
        - Added strong named library.

        [SDK]
        - Added Sample Codes
            * Added sample codes for setting or initializing USB serial number.

    Ver. 5.5.0
    01/31/2018
        [StarIO]
            - Fixed a bug that printing fails when printer restores within timeout after turning off printer power
            under monitoring status in the LAN interface.

        [StarIOExtension]
        - Added method for setting or clearing horizontal tab.

        [SDK]
        - Added Sample Codes.
            * Implementation of the CJK unified ideographs printing sample under UTF-8.
            (Supported by TSP650II (JP2/TW) with firmware version 4.0 or later only)

    Ver. 5.4.1
    10/26/2017
        Update library obfuscation license

    Ver. 5.4.0
    10/12/2017
        First Release
