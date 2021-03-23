using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpListnerPosAsServerDemo
{
    public static class SocketDataHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static RequestObject RequestObject { get; set; }

        public static void ProcessRequest(IAsyncResult ar)
        {

            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client;
            try
            {
                client = listener.EndAcceptTcpClient(ar);

                // keep listening
                listener.BeginAcceptTcpClient(new AsyncCallback(ProcessRequest), listener);

                var rp = new ClientHandler();
                RequestObject = new RequestObject();
                RequestObject.Amout = "5000";
                RequestObject.OrderNr = "10000000000000000000666666";
                rp.RequestObject = RequestObject;

                rp.Proccess(client);

            }
            catch (SocketException ex)
            {
                logger.Error("Error accepting TCP connection: {0}", ex.Message);

                //Invoke event if need
                return;
            }
            catch (ObjectDisposedException)
            {
                // The listener was Stop()'d, disposing the underlying socket and
                // triggering the completion of the callback. We're already exiting,
                // so just return.

                //TODO: Log this Exeption
                logger.Error("Listen canceled.");
                return;
            }
        }

    }
}
