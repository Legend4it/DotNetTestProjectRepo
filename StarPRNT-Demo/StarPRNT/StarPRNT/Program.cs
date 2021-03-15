using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StarPRNT
{
    class Program
    {
        public static IPort port;
        private static Communication.PeripheralStatus barcodeReaderStatus;
        private static Thread monitoringBarcodeReaderThread;


        static void Main(string[] args)
        {
            var version = Factory.I.GetStarIOVersion();
            Console.WriteLine($"Version:{version}");

            foreach(PortInfo item in Factory.I.SearchPrinter(PrinterInterfaceType.USBPrinterClass))
            {
                Console.WriteLine($"ModelName:{item.ModelName},PortName:{item.PortName},USBSerialNumber:{item.USBSerialNumber}");
                port = Factory.I.GetPort(item.PortName, "USBPRN", 1000);
            }

            while (true)
            {
                ClearBarcodeReaderBuffer();

                // get the user input for every iteration, allowing to exit at will
                ConsoleKeyInfo line = Console.ReadKey();
                if (line.Key==ConsoleKey.Escape)
                {
                    // exit the method.
                    return; // use "break" if you just want to exit the loop
                }
                MonitoringBarcodeReader();
            }
        }
        private static void ClearBarcodeReaderBuffer()
        {
            byte[] clearBufferCommands = BcrFunctions.CreateClearBuffer();

            port.WritePort(clearBufferCommands, 0, (uint)clearBufferCommands.Length);
        }

        public static void MonitoringBarcodeReader()
        {
            var resultData = string.Empty;
            // Create IPeripheralConnectParser object.
            IPeripheralConnectParser parser = StarIoExt.CreateBcrConnectParser(BcrModel.POP1);

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);
            CheckBarcodeReaderStatus();
            
            //Console.WriteLine(barcodeReaderStatus);

            if (barcodeReaderStatus == Communication.PeripheralStatus.Connect) // Barcode reader is connected.
            {
                resultData=ReadBarcodeReaderData();  // Read barcode reader data.
                //Console.WriteLine(barcodeReaderStatus);
                Console.WriteLine(resultData);
            }
            //return resultData;

        }
        private static string ReadBarcodeReaderData()
        {
            var returnResult = string.Empty;
            // Barcode reader data parser sample is in "BcrDataParser" class.
            BcrDataParser parser = new BcrDataParser();

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);

            //if (result.Result != Communication.Result.Success)
            //{
            //    OnBarcodeReaderImpossible();

            //    return;
            //}

            // Check parser property value.
            byte[] barcodeData = parser.Data;

            if (barcodeData != null) // Barcode reader data is not empty.
            {
                //OnBarcodeDataReceived(barcodeData);
                string text = Encoding.UTF8.GetString(barcodeData, 0, barcodeData.Length);
                returnResult = text;
            }
            return returnResult;
        }

        private static void CheckBarcodeReaderStatus()
        {
            // Create IPeripheralConnectParser object.
            IPeripheralConnectParser parser = StarIoExt.CreateBcrConnectParser(BcrModel.POP1);

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);

            // Check peripheral status.
            barcodeReaderStatus = Communication.PeripheralStatus.Invalid;
            if (result.Result == Communication.Result.Success)
            {
                // Check parser property value.
                if (parser.IsConnected) // connect
                {
                    barcodeReaderStatus = Communication.PeripheralStatus.Connect;
                }
                else // disconnect
                {
                    barcodeReaderStatus = Communication.PeripheralStatus.Disconnect;
                }
            }
            else // communication error
            {
                barcodeReaderStatus = Communication.PeripheralStatus.Impossible;
            }

            //switch (barcodeReaderStatus)
            //{
            //    default:
            //    case Communication.PeripheralStatus.Impossible:
            //        OnBarcodeReaderImpossible();
            //        break;

            //    case Communication.PeripheralStatus.Connect:
            //        OnBarcodeReaderConnect();
            //        break;

            //    case Communication.PeripheralStatus.Disconnect:
            //        OnBarcodeReaderDisconnect();
            //        break;
            //}
        }


    }
}

public static class BcrFunctions
{
    public static byte[] CreateClearBuffer()
    {
        // Barcode reader command builder sample is in "BcrBuilder" class.
        BcrBuilder builder = new BcrBuilder();

        builder.AppendClearBuffer();

        return builder.Commands;
    }
}
public class BcrBuilder
{
    private List<byte> commandsList;

    public BcrBuilder()
    {
        commandsList = new List<byte>();
    }

    public byte[] Commands
    {
        get
        {
            return commandsList.ToArray();
        }
    }

    public void Append(byte data)
    {
        commandsList.Add(data);
    }

    public void Append(byte[] data)
    {
        commandsList.AddRange(data);
    }

    public void AppendClearBuffer()
    {
        Append(new byte[] { 0x1b, 0x1d, (byte)'B', 0x33 });
    }
}

public class CommunicationResult
{
    public Communication.Result Result { get; set; }

    public int Code { get; set; }

    public CommunicationResult()
    {
        Result = Communication.Result.ErrorUnknown;
        Code = StarResultCode.ErrorFailed;
    }
}


public static class Communication
{

    public enum Result
    {
        Success,
        ErrorUnknown,
        ErrorOpenPort,
        ErrorBeginCheckedBlock,
        ErrorEndCheckedBlock,
        ErrorWritePort,
        ErrorReadPort,
    }

    public enum PeripheralStatus
    {
        Invalid,
        Impossible,
        Connect,
        Disconnect
    }
    public static CommunicationResult ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)
    {
        Result result = Result.ErrorUnknown;
        int code;

        try
        {
            result = Result.ErrorOpenPort;

            if (port == null)
            {
                throw new PortException("port is null.");
            }

            result = Result.ErrorWritePort;

            StarPrinterStatus printerStatus = port.GetParsedStatus();

            byte[] commands = parser.SendCommands;

            port.WritePort(commands, 0, (uint)commands.Length);

            result = Result.ErrorReadPort;

            byte[] readBuffer = new byte[1024];
            List<byte> allReceiveData = new List<byte>();

            uint startDate = (uint)Environment.TickCount;

            while (true)
            {
                if ((UInt32)Environment.TickCount - startDate >= 1000) // Timeout
                {
                    throw new PortException("ReadPort timeout.");
                }

                Thread.Sleep(10);

                uint receiveSize = port.ReadPort(ref readBuffer, 0, (uint)readBuffer.Length);

                if (receiveSize == 0)
                {
                    continue;
                }

                byte[] receiveData = new byte[receiveSize];
                Array.Copy(readBuffer, 0, receiveData, 0, receiveSize);

                allReceiveData.AddRange(receiveData);

                if (parser.Parse(allReceiveData.ToArray(), allReceiveData.Count) == ParseResult.Success)
                {
                    result = Result.Success;
                    code = StarResultCode.Succeeded;

                    break;
                }
            }

        }
        catch (PortException ex)
        {
            code = ex.ErrorCode;
        }

        return new CommunicationResult()
        {
            Result = result,
            Code = code
        };
    }

}

public class BcrDataParser : IPeripheralCommandParser
{
    private static readonly int HEADER_LENGTH = 6;

    public BcrDataParser()
    {
        Data = new byte[0];
    }

    public byte[] Data { get; private set; }

    public virtual ParseResult Parse(byte[] response, int responseLength)
    {
        if (responseLength < HEADER_LENGTH)
        {
            return ParseResult.Invalid;
        }

        int parseStartIndex = 0;
        int dataLength = 0;
        bool canParse = false;

        for (int i = 0; i <= responseLength - HEADER_LENGTH; i++)
        {
            if (response[i] == 0x1b &&
                response[i + 1] == 0x1d &&
                response[i + 2] == (byte)'B' &&
                response[i + 3] == 0x32)
            {
                int length = (response[i + 4] & 0xFF) + (response[i + 5] & 0xFF) * 0x100; // k = n1 + n2 * 256

                int requiredResponseLength = i + HEADER_LENGTH + length;

                if (requiredResponseLength <= responseLength)
                {
                    parseStartIndex = i + HEADER_LENGTH;
                    dataLength = length;
                    canParse = true;
                }
            }
        }

        if (!canParse)
        {
            return ParseResult.Invalid;
        }

        if (dataLength == 0) // no data
        {
            return ParseResult.Success;
        }

        byte[] receiveData = new byte[dataLength];

        Array.Copy(response, parseStartIndex, receiveData, 0, dataLength);

        Data = receiveData;

        return ParseResult.Success;
    }

    public byte[] SendCommands
    {
        get
        {
            return new byte[] { 0x1b, 0x1d, (byte)'B', 0x32 };    // Request barcode data command.
        }
    }

    public byte[] ReceiveCommands
    {
        get
        {
            return null; // no use
        }
    }
}

