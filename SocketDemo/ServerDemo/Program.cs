using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClientListener
{
    class Program
    {
        static int Main(string[] args)
        {
            StartListening(Dns.GetHostName(), 11000);
            //StartListening("127.0.0.1", 11000);
            return 0;
        }

        // Incoming data from the client.  
        public static string data = null;

        public static void StartListening(string host, int port)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            Console.WriteLine($"Server IP:{ipAddress}");

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }



        //public static void StartListening()
        //{
        //    Console.Title = "Socket Server";
        //    Console.WriteLine("Listening for messages...");

        //    Socket serverSock = new Socket(
        //        AddressFamily.InterNetwork,
        //        SocketType.Stream,
        //        ProtocolType.Tcp);

        //    IPAddress serverIP = IPAddress.Any;
        //    IPEndPoint serverEP = new IPEndPoint(serverIP, 11000);

        //    //SocketPermission perm = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "98.112.235.18", 11000);
        //    serverSock.Bind(serverEP);
        //    serverSock.Listen(10);

        //    while (true)
        //    {
        //        Socket connection = serverSock.Accept();

        //        Byte[] serverBuffer = new Byte[8];
        //        String message = String.Empty;

        //        while (connection.Available >= 0)
        //        {
        //            int bytes = connection.Receive(
        //                serverBuffer,
        //                serverBuffer.Length,
        //                0);

        //            message += Encoding.UTF8.GetString(
        //                serverBuffer,
        //                0,
        //                bytes);
        //        }

        //        Console.WriteLine(message);
        //        connection.Close();
        //    }
        //}

    }
}
