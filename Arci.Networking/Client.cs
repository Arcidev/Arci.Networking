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
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;
        private const int maxBufferSize = 10000;

        /// <summary>
        /// Symmetric encryptor
        /// </summary>
        public ISymmetricEncryptor Encryptor { get; set; }

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="client">TcpClient connected to the server</param>
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
            var client = new TcpClient();

            // check connection
            try
            {
                await client.ConnectAsync(server, port).ConfigureAwait(false);
                return new Client(client);
            }
            catch (SocketException)
            {
                client.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Sends packet to server
        /// </summary>
        /// <param name="packet">Packet to send</param>
        public void SendPacket(Packet packet) => SendPacket(packet, true);

        /// <summary>
        /// Sends packet to server
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="encrypt">Encrypts data with Aes key if set</param>
        public void SendPacket(Packet packet, bool encrypt)
        {
            var data = PrepareDataToSend(packet.Data, encrypt);
            if (data != null)
                stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Sends packet to server asynchronously
        /// </summary>
        /// <param name="packet">Packet to send</param>
        public async Task SendPacketAsync(Packet packet) => await SendPacketAsync(packet, true).ConfigureAwait(false);

        /// <summary>
        /// Sends packet to server asynchronously
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="encrypt">Encrypts data with Aes key if set</param>
        public async Task SendPacketAsync(Packet packet, bool encrypt)
        {
            var data = PrepareDataToSend(packet.Data, encrypt);
            if (data != null)
                await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
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
        /// <returns>List of packets received from server. Blocks thread until data become available.</returns>
        public async Task<IEnumerable<Packet>> ReceiveDataAsync(bool decrypt) => await ReceiveDataAsync(decrypt, null).ConfigureAwait(false);

        /// <summary>
        /// Receive data as a list of packets from server
        /// </summary>
        /// <param name="decrypt">Decrypt data with Aes key if set</param>
        /// <param name="token">Token to cancel awaited reading</param>
        /// <returns>List of packets received from server. Blocks thread until data become available.</returns>
        public async Task<IEnumerable<Packet>> ReceiveDataAsync(bool decrypt, CancellationToken? token)
        {
            return TransformStreamToPackets(await ReceiveDataAsync(token).ConfigureAwait(false), decrypt);
        }

        /// <summary>
        /// Receives data as byte stream
        /// </summary>
        /// <returns>Byte stream of received data. If there are no data to receive null is returned</returns>
        public byte[] ReceiveData()
        {
            return stream.DataAvailable ? ReadData() : null;
        }

        /// <summary>
        /// Receives data as byte stream asynchronously
        /// </summary>
        /// <returns>Byte stream of received data. Blocks thread until data become available.</returns>
        public async Task<byte[]> ReceiveDataAsync() => await ReceiveDataAsync(null).ConfigureAwait(false);

        /// <summary>
        /// Receives data as byte stream asynchronously
        /// </summary>
        /// <param name="token">Token to cancel awaited reading</param>
        /// <returns>Byte stream of received data. Blocks thread until data become available.</returns>
        public async Task<byte[]> ReceiveDataAsync(CancellationToken? token)
        {
            return await ReadDataAsync(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Free all resources
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
            tcpClient.Dispose();
        }

        private byte[] PrepareDataToSend(byte[] data, bool encrypt)
        {
            var result = encrypt && Encryptor != null ? Encryptor.Encrypt(data) : data;
            if (result == null || !result.Any())
                return null;

            return BitConverter.GetBytes((UInt16)result.Length).Concat(result).ToArray();
        }

        private byte[] ReadData()
        {
            var data = new byte[maxBufferSize];
            int length = stream.Read(data, 0, maxBufferSize);
            if (length == 0)
                return null;

            return data.Take(length).ToArray();
        }

        private async Task<byte[]> ReadDataAsync(CancellationToken? token)
        {
            var data = new byte[maxBufferSize];
            int length = await stream.ReadAsync(data, 0, maxBufferSize, token ?? CancellationToken.None);
            if (length == 0)
                return null;

            return data.Take(length).ToArray();
        }

        private IEnumerable<Packet> TransformStreamToPackets(byte[] data, bool decrypt)
        {
            if (data == null)
                return null;

            var packets = new List<Packet>();
            var lengthRead = 0;
            while (data.Length > lengthRead)
            {
                // Fetch packet length and data if missing
                if (data.Length - lengthRead < sizeof(UInt16) && !AddPendingData(ref data, ref lengthRead))
                    return packets;

                var length = BitConverter.ToUInt16(data, lengthRead);

                // Fetch packet data if missing
                if (data.Length - lengthRead < length && !AddPendingData(ref data, ref lengthRead))
                    return packets;

                var packetData = new byte[length];
                Array.Copy(data, lengthRead + sizeof(UInt16), packetData, 0, length);

                var packet = new Packet(decrypt && Encryptor != null ? Encryptor.Decrypt(packetData) : packetData);
                packets.Add(packet);
                lengthRead += sizeof(UInt16) + length;
            }

            return packets;
        }

        /// <summary>
        /// Adds pending data to existing data while skipping already read data
        /// </summary>
        /// <param name="data">Current data where pending data will be added</param>
        /// <param name="lengthRead">Length of current data that has already been read</param>
        /// <returns>True if pending data were added otherwise false</returns>
        private bool AddPendingData(ref byte[] data, ref int lengthRead)
        {
            var pendingData = ReceiveData();
            if (pendingData == null)
                return false;

            data = data.Skip(lengthRead).Concat(pendingData).ToArray();
            lengthRead = 0;

            return true;
        }
    }
}
