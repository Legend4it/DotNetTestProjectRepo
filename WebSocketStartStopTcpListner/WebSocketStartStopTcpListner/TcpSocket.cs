using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketStartStopTcpListner
{
    public class TcpSocket:IDisposable
    {
        #region Fields.
        private CancellationToken _ct;
        private CancellationTokenSource _cts;
        private TcpListener _listener;

        #endregion

        #region Public.
        /// <summary>
        /// Create new instance of TcpServer.
        /// </summary>
        /// <param name="ip">Ip of server.</param>
        /// <param name="port">Port of server.</param>
        public TcpSocket()
        {
            _ct = new CancellationToken();
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Parse("192.168.1.163"), 11000);

        }

        public void Dispose()
        {
            _ct = new CancellationToken();
            _cts = null;
            _listener = null;
        }

        /// <summary>
        /// Starts receiving incoming requests.
        /// </summary>
        public void Start()
        {
            _listener.Start();
            _ct = _cts.Token;
            _listener.BeginAcceptTcpClient(ProcessRequest, _listener);
        }

        /// <summary>
        /// Stops receiving incoming requests.
        /// </summary>
        public void Stop()
        {
            ////If listening has been cancelled, simply go out from method.
            //if (_ct.IsCancellationRequested)
            //{
            //    return;
            //}

            //Cancels listening.
            _cts.Cancel();

            //Waits a little, to guarantee that all operation receive information about cancellation.
            Thread.Sleep(100);
            _listener.Stop();
            //this.Dispose();
        }
        #endregion

        #region Private.
        //Process single request.
        private void ProcessRequest(IAsyncResult ar)
        {
            TcpListener l = (TcpListener)ar.AsyncState;
            TcpClient c;
            try
            {
                c = l.EndAcceptTcpClient(ar);
                // keep listening
                l.BeginAcceptTcpClient(new AsyncCallback(ProcessRequest), l);
                var rp = new RequestProcessor();
                rp.Proccess(c);

            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error accepting TCP connection: {0}", ex.Message);

                // unrecoverable
                //_doneEvent.Set();
                return;
            }
            catch (ObjectDisposedException)
            {
                // The listener was Stop()'d, disposing the underlying socket and
                // triggering the completion of the callback. We're already exiting,
                // so just return.
                Console.WriteLine("Listen canceled.");
                return;
            }
            ////Stop if operation was cancelled.
            //if (_ct.IsCancellationRequested)
            //{
            //    return;
            //}

            //var listener = ar.AsyncState as TcpListener;
            //if (listener == null)
            //{
            //    return;
            //}

            ////Check cancellation again. Stop if operation was cancelled.
            //if (_ct.IsCancellationRequested)
            //{
            //    return;
            //}

            ////Starts waiting for the next request.
            //listener.BeginAcceptTcpClient(ProcessRequest, listener);


            ////Gets client and starts processing received request.
            //using (TcpClient client = listener.EndAcceptTcpClient(ar))
            //{
            //    var rp = new RequestProcessor();
            //    rp.Proccess(client);
            //}
        }
        #endregion

    }

}
