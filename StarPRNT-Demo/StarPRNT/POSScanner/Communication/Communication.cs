using StarMicronics.StarIO;
using StarMicronics.StarIOExtension;
using System;
using System.Collections.Generic;
using System.Threading;
using static POPScanner.EnumSource;

namespace POPScanner
{
    public class Communication
    {
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

                    uint startDate = (uint)Environment.TickCount; //TODO: Use thread in main class

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
}
