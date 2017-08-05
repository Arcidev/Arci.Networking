using System;
using System.Linq;
using System.Net.Sockets;
using Arci.Networking.Data;
using Arci.Networking.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Arci.Networking
{
    /// <summary>
    /// Client instance
    /// </summary>
    public class Client : IDisposable
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="client">TcpClient to be used</param>
        public Client(TcpClient client)
        {
            tcpClient = client;
            stream = tcpClient.GetStream();
        }

        /// <summary>
        /// Creates new network instance
        /// </summary>
        /// <param name="server">Ip adress of the server</param>
        /// <param name="port">Port of the server</param>
        /// <returns>New instance of client</returns>
        public static async Task<Client> CreateAsync(string server, int port)
        {
            TcpClient client = null;

            // check connection
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(server, port);
            }
            catch (SocketException)
            {
                return null;
            }

            return new Client(client);
        }

        /// <summary>
        /// Sends packet to server
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="encrypt">Encrypts data with Aes key if set</param>
        public void SendPacket(Packet packet, bool encrypt = true)
        {
            var data = encrypt && Encryptor != null ? Encryptor.Encrypt(packet.Data) : packet.Data;
            if (data == null || !data.Any())
                return;

            var toSend = BitConverter.GetBytes((UInt16)data.Length).Concat(data).ToArray();
            stream.Write(toSend, 0, data.Length + sizeof(UInt16));
        }

        /// <summary>
        /// Sends packet to server asynchronously
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="encrypt">Encrypts data with Aes key if set</param>
        public async Task SendPacketAsync(Packet packet, bool encrypt = true)
        {
            var data = encrypt && Encryptor != null ? Encryptor.Encrypt(packet.Data) : packet.Data;
            if (data == null || !data.Any())
                return;

            var toSend = BitConverter.GetBytes((UInt16)data.Length).Concat(data).ToArray();
            await stream.WriteAsync(toSend, 0, data.Length + sizeof(UInt16));
        }

        /// <summary>
        /// Receive data as a list of packets from server
        /// </summary>
        /// <param name="decrypt">Decrypt data with Aes key if set</param>
        /// <returns>List of packets received from server. If there are no data to receive null is returned</returns>
        public IEnumerable<Packet> ReceiveData(bool decrypt)
        {
            return TransformStreamToPackets(ReceiveData(), decrypt);
        }

        /// <summary>
        /// Receive data as a list of packets from server
        /// </summary>
        /// <param name="decrypt">Decrypt data with Aes key if set</param>
        /// <param name="token">Token to cancel awaited reading</param>
        /// <returns>List of packets received from server. Blocks thread until data become available.</returns>
        public async Task<IEnumerable<Packet>> ReceiveDataAsync(bool decrypt, CancellationToken? token = null)
        {
            return TransformStreamToPackets(await ReceiveDataAsync(token), decrypt);
        }

        /// <summary>
        /// Receives data as byte stream
        /// </summary>
        /// <returns>Byte stream of received data. If there are no data to receive null is returned</returns>
        public byte[] ReceiveData()
        {
            if (stream.DataAvailable)
                return ReadData();

            return null;
        }

        /// <summary>
        /// Receives data as byte stream asynchronously
        /// </summary>
        /// <param name="token">Token to cancel awaited reading</param>
        /// <returns>Byte stream of received data. Blocks thread until data become available.</returns>
        public async Task<byte[]> ReceiveDataAsync(CancellationToken? token = null)
        {
            return await ReadDataAsync(token);
        }

        /// <summary>
        /// Symmetric encryptor
        /// </summary>
        public ISymmetricEncryptor Encryptor { get; set; }

        /// <summary>
        /// Free all resources
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
            tcpClient.Dispose();
        }

        private byte[] ReadData()
        {
            var data = new byte[Packet.MaxPacketSize];
            int length = stream.Read(data, 0, Packet.MaxPacketSize);
            if (length == 0)
                return null;

            return data.Take(length).ToArray();
        }

        private async Task<byte[]> ReadDataAsync(CancellationToken? token)
        {
            var data = new byte[Packet.MaxPacketSize];
            int length = token.HasValue ? await stream.ReadAsync(data, 0, Packet.MaxPacketSize, token.Value) : await stream.ReadAsync(data, 0, Packet.MaxPacketSize);
            if (length == 0)
                return null;

            return data.Take(length).ToArray();
        }

        private IEnumerable<Packet> TransformStreamToPackets(byte[] data, bool decrypt)
        {
            if (data == null)
                return null;

            List<Packet> packets = new List<Packet>();
            while (data.Any())
            {
                var length = BitConverter.ToUInt16(data, 0);
                data = data.Skip(sizeof(UInt16)).ToArray();
                var packetData = data.Take(length).ToArray();
                Packet packet = new Packet(decrypt && Encryptor != null ? Encryptor.Decrypt(packetData) : packetData);
                packets.Add(packet);
                data = data.Skip(length).ToArray();
            }

            return packets;
        }
    }
}
