using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClient
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Enter server IP-Adress:");
            
            StartClient(Console.ReadLine(), 11000);
            
            
            //StartClient(Dns.GetHostName(), 11000);
            //StartClient("127.0.0.1", 11000);
            return 0;
        }

        public static void StartClient(string host, int port)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        //public static void StartClient()
        //{
        //    Design the client a bit
        //    Console.Title = "Socket Client";

        //    Console.Write("Enter the IP of the server: ");
        //    IPAddress clientIP = IPAddress.Parse(Console.ReadLine());
        //    String message = String.Empty;

        //    while (true)
        //    {
        //        Console.Write("Enter the message to send: ");
        //        The messsage to send
        //        message = Console.ReadLine();

        //        IPEndPoint clientEP = new IPEndPoint(clientIP, 11000);

        //        Setup the socket
        //       Socket clientSock = new Socket(
        //           AddressFamily.InterNetwork,
        //           SocketType.Stream,
        //           ProtocolType.Tcp);

        //        Attempt to establish a connection to the server
        //        Console.Write("Establishing connection to the server... ");
        //        try
        //        {
        //            clientSock.Connect(clientEP);

        //            Send the message
        //            clientSock.Send(Encoding.UTF8.GetBytes(message));
        //            clientSock.Shutdown(SocketShutdown.Both);
        //            clientSock.Close();
        //            Console.Write("Message sent successfully.\n\n");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //}


    }
}
