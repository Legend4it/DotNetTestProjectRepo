using POPScanner.Bcr;
using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.IO;
using System.Text;
using static POPScanner.EnumSource;

namespace POPScanner
{
    public static class BarCodeFactory
    {
        private static PeripheralStatus barcodeReaderStatus;

        public static void ClearBarcodeReaderBuffer(IPort port)
        {
            byte[] clearBufferCommands = BcrFunctions.CreateClearBuffer();
            port.WritePort(clearBufferCommands, 0, (uint)clearBufferCommands.Length);
        }
        public static string MonitoringBarcodeReader(IPort port)
        {
            var result = string.Empty;

            // Create IPeripheralConnectParser object.
            IPeripheralConnectParser parser = StarIoExt.CreateBcrConnectParser(BcrModel.POP1);

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult comResult = Communication.ParseDoNotCheckCondition(parser, port);
            CheckBarcodeReaderStatus(port);

            //Console.WriteLine(barcodeReaderStatus);

            if (barcodeReaderStatus == PeripheralStatus.Connect) // Barcode reader is connected.
            {
                result = ReadBarcodeReaderData(port);  // Read barcode reader data.
            }
            return result;
        }
        public static string ReadBarcodeReaderData(IPort port)
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
        public static void CheckBarcodeReaderStatus(IPort port)
        {
            // Create IPeripheralConnectParser object.
            IPeripheralConnectParser parser = StarIoExt.CreateBcrConnectParser(BcrModel.POP1);

            // Usage of parser sample is "Communication.ParseDoNotCheckCondition(IPeripheralCommandParser parser, IPort port)".
            CommunicationResult result = Communication.ParseDoNotCheckCondition(parser, port);

            // Check peripheral status.
            barcodeReaderStatus = PeripheralStatus.Invalid;
            if (result.Result == Result.Success)
            {
                // Check parser property value.
                if (parser.IsConnected) // connect
                {
                    barcodeReaderStatus = PeripheralStatus.Connect;
                }
                else // disconnect
                {
                    barcodeReaderStatus = PeripheralStatus.Disconnect;
                }
            }
            else // communication error
            {
                barcodeReaderStatus = PeripheralStatus.Impossible;
            }

            //switch (barcodeReaderStatus)
            //{
            //    default:
            //    case PeripheralStatus.Impossible:
            //        OnBarcodeReaderImpossible();
            //        break;

            //    case PeripheralStatus.Connect:
            //        OnBarcodeReaderConnect();
            //        break;

            //    case PeripheralStatus.Disconnect:
            //        OnBarcodeReaderDisconnect();
            //        break;
            //}
        }

        private static void OnBarcodeReaderDisconnect()
        {
            throw new NotImplementedException();
        }

        private static void OnBarcodeReaderConnect()
        {
            throw new NotImplementedException();
        }

        private static void OnBarcodeReaderImpossible()
        {
            throw new NotImplementedException();
        }
    }
}
