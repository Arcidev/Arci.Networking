﻿using System;
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
        private readonly TcpListener server;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="address">An IPAddress that represents the local IP address</param>
        /// <param name="port">The port on which to listen for incoming connection attempts</param>
        public Server(IPAddress address, int port)
        {
            server = new TcpListener(address, port);
            server.Start();
        }

        /// <summary>
        /// Accepts new client
        /// </summary>
        /// <returns>New Tcp client if pending. Otherwise null</returns>
        public async Task<TcpClient> AcceptClient()
        {
            return server.Pending() ? await server.AcceptTcpClientAsync().ConfigureAwait(false) : null;
        }

        /// <summary>
        /// Accepts new client with block waiting
        /// </summary>
        /// <returns>New Tcp client</returns>
        public async Task<TcpClient> AcceptClientAsync()
        {
            return await server.AcceptTcpClientAsync().ConfigureAwait(false);
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
