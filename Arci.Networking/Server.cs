using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
        public async Task<TcpClient> AcceptClient()
        {
            if (server.Pending())
                return await server.AcceptTcpClientAsync();

            return null;
        }

        /// <summary>
        /// Accepts new client with block waiting
        /// </summary>
        /// <returns>New Tcp client</returns>
        public async Task<TcpClient> AcceptClientAsync()
        {
            return await server.AcceptTcpClientAsync();
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
