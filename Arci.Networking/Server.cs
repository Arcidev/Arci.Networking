using System;
using System.Net;
using System.Net.Sockets;

namespace Arci.Networking
{
    public class Server : IDisposable
    {
        private TcpListener server;

        public Server(int port)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
        }

        public TcpClient AcceptClient()
        {
            if (server.Pending())
                return server.AcceptTcpClient();

            return null;
        }

        public TcpClient AcceptClientBlockWait()
        {
            return server.AcceptTcpClient();
        }

        public void Dispose()
        {
            server.Stop();
        }
    }
}
