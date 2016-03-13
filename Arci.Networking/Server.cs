using System;
using System.Net;
using System.Net.Sockets;

namespace Arci.Networking
{
    /// <summary>
    /// Server instance
    /// </summary>
    public class Server : IDisposable
    {
        private TcpListener server;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="port">Port where it should be atached</param>
        public Server(int port)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
        }

        /// <summary>
        /// Accepts new client
        /// </summary>
        /// <returns>New Tcp client if pending. Otherwise null</returns>
        public TcpClient AcceptClient()
        {
            if (server.Pending())
                return server.AcceptTcpClient();

            return null;
        }

        /// <summary>
        /// Accepts new client with block waiting
        /// </summary>
        /// <returns>New Tcp client</returns>
        public TcpClient AcceptClientBlockWait()
        {
            return server.AcceptTcpClient();
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            server.Stop();
        }
    }
}
