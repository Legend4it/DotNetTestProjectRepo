using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpListnerPosAsServerDemo
{
    public class ConnectionManager : IDisposable
    {
        private CancellationToken _ct;
        private CancellationTokenSource _cts;
        private TcpListener _listener;
        private Logger logger = LogManager.GetCurrentClassLogger(typeof(ConnectionManager));

        public ConnectionManager(int port)
        {
            InitializeConnection(port);
        }

        public bool CheckConnection()
        {
            return _listener.Pending(); //Need to test
        }
        public void InitializeConnection(int port)
        {
            _ct = new CancellationToken();
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, port);
            //_listener = TcpListener.Create(port);

        }

        public void Start()
        {
            _listener.Start();
            _ct = _cts.Token;
            _listener.BeginAcceptTcpClient(SocketDataHandler.ProcessRequest, _listener);
        }

        public void Stop()
        {

            //Cancels listening.
            _cts.Cancel();

            //Waits a little, to guarantee that all operation receive information about cancellation.
            Thread.Sleep(100);
            _listener.Stop();
        }

        public void Dispose()
        {
            _ct = new CancellationToken();
            _cts = null;
            _listener = null;
        }
    }

}