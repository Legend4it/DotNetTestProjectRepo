using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//Client Demo APP
namespace WebSocketClient
{


    public class RequestObject
    {
        public string OrderNr { get; set; } = "12345";
        public string Amout { get; set; } = "100";
        public string PartnerId { get; set; } = "1";
        public string EcrId { get; set; } = "EcrA";
        public string RequestType { get; set; } = string.Empty;
        public string ResultObject { get; set; } = string.Empty;
        public string ApprovalCode { get; internal set; }
        public bool Refound { get; set; } = false;

    }
    class Program
    {
        private const string IPADRESS = "172.31.79.228";
        private const string ISREADY = "ISREADY";
        private const string GETPAYMENT = "GETPAYMENT";
        private const int PORT = 11000;

        static void Main(string[] args)
        {
            for (; ; ) //Infinity loop
            {
                if (!IsConnected())
                {
                    continue;
                }
                if (IsPaymentReady())
                {
                    //Send Get Payment data
                    SendRequest(IPADRESS, GETPAYMENT);
                }
            }

            //var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new RequestObject());
            ////SendRequest("192.168.0.66", jsonMessage);

            ////Thread.Sleep(500);

            ////jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new RequestObject() { RequestType = "Result" });
            ////SendRequest("192.168.0.66", jsonMessage);

            ////Thread.Sleep(500);

            //jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new RequestObject() { OrderNr = "1003192192", Refound = false });
            ////SendRequest("185.248.53.20", jsonMessage);
            ////SendRequest("172.31.79.183", jsonMessage);
            //SendRequest("192.168.1.168", jsonMessage);

            //Thread.Sleep(500);

            //jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new RequestObject() { OrderNr = "1003192192", RequestType = "Result" });
            ////SendRequest("185.248.53.20", jsonMessage);
            ////SendRequest("172.31.79.183", jsonMessage);
            //SendRequest("192.168.1.168", jsonMessage);


            //Thread.Sleep(500);

            Console.ReadKey();
        }

        static void SendRequest(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                //Int32 port = 13000; //Terminal Port
                Int32 port = 11000; //Test Port
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[5000];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
        static bool SendCheckRequest(String server, String message)
        {
            try
            {
                Int32 port = 11000; //Test Port
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[5000];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                if (responseData.ToUpper().Equals(ISREADY))
                {
                    return true;
                }

                // Close everything.
                stream.Close();
                client.Close();

                return false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return false;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return false;
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }


        static bool IsPaymentReady()
        {
            return SendCheckRequest(IPADRESS, "ISREADY");
        }
        static bool IsConnected()
        {
            try
            {
                //Or check if Client IsBound True, in case of Client is redan connected or Client ready
                TcpClient client = new TcpClient();
                client.Connect(IPADRESS, PORT);
                client.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
