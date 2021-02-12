using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpListnerIpFromHost
{
    class Program
    {
        static void Main(string[] args)
        {

            IPHostEntry server = Dns.GetHostEntry("posdev.fortusinternational.com");
            IPAddress ipAddress = server.AddressList[0];

            Console.WriteLine("listening on {0}, port {1}", ipAddress, "13000");
            TcpListener listener = new TcpListener(IPAddress.Any, 13000);
            Socket listenerSocket = listener.Server;

            //LingerOption lingerOption = new LingerOption(true, 10);
            //listenerSocket.SetSocketOption(SocketOptionLevel.Socket,
            //                  SocketOptionName.Linger,
            //                  lingerOption);

            // start listening and process connections here.
            listener.Start();


            TcpClient tcpClient = new TcpClient();
            IPAddress ipAddress2 = Dns.GetHostEntry("posdev.fortusinternational.com").AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress2, 11000);

            tcpClient.Connect(ipEndPoint);
        }
    }
}
