using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WebSocketStartStopTcpListner
{
    public class TcpSocket02
    {
        public void Listtener()
        {
            Console.WriteLine("Starting...");
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.0.66"), 13000);
            server.Start();
            Console.WriteLine("Started.");
            while (true)
            {
                try
                {
                    ClientWorking cw = new ClientWorking(server.AcceptTcpClient());
                    new Thread(new ThreadStart(cw.DoSomethingWithClient)).Start();
                }
                catch (Exception)
                {
                    //throw;
                    break;
                }
            }
            server.Stop();
        }
    }
} 
public class ClientWorking
{
    private Stream ClientStream;
    private TcpClient Client;

    public ClientWorking(TcpClient Client)
    {
        this.Client = Client;
        ClientStream = Client.GetStream();
    }

    public void DoSomethingWithClient()
    {
        StreamWriter sw = new StreamWriter(ClientStream);
        StreamReader sr = new StreamReader(sw.BaseStream);
        sw.WriteLine("Hi. This is x2 TCP/IP easy-to-use server");
        sw.Flush();
        string data;
        try
        {
            while ((data = sr.ReadLine()) != "exit")
            {
                sw.WriteLine(data);
                sw.Flush();
            }
        }
        finally
        {
            sw.Close();
        }
    }
}
