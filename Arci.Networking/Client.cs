using System;
using System.Linq;
using System.Net.Sockets;
using Arci.Networking.Data;
using Arci.Networking.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arci.Networking
{
    /// <summary>
    /// Client instance
    /// </summary>
    public class Client : IDisposable
    {
        private TcpClient tcpClnt;
        private NetworkStream stream;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="client">TcpClient to be used</param>
        public Client(TcpClient client)
        {
            tcpClnt = client;
            stream = tcpClnt.GetStream();
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
            var data = encrypt && AesEncryptor != null ? AesEncryptor.Encrypt(packet.Data) : packet.Data;
            var toSend = BitConverter.GetBytes((UInt16)data.Length).Concat(data).ToArray();

            stream.Write(toSend, 0, data.Length + sizeof(UInt16));
        }

        /// <summary>
        /// Receive data from server
        /// </summary>
        /// <param name="decrypt">Decrypt data with Aes key if set</param>
        /// <returns>List of packets received from server</returns>
        public IEnumerable<Packet> ReceiveData(bool decrypt)
        {
            if (stream.DataAvailable)
            {
                byte[] data = readData();
                List<Packet> packets = new List<Packet>();
                while (data.Any())
                {
                    var length = BitConverter.ToUInt16(data, 0);
                    data = data.Skip(sizeof(UInt16)).ToArray();
                    var packetData = data.Take(length).ToArray();
                    Packet packet = new Packet(decrypt && AesEncryptor != null ? AesEncryptor.Decrypt(packetData) : packetData);
                    packets.Add(packet);
                    data = data.Skip(length).ToArray();
                }

                return packets;
            }

            return null;
        }

        /// <summary>
        /// Receives data as byte stream
        /// </summary>
        /// <returns>Byte stream of received data</returns>
        public byte[] ReceiveData()
        {
            if (stream.DataAvailable)
                return readData();

            return null;
        }

        /// <summary>
        /// Aes encryptor
        /// </summary>
        public AesEncryptor AesEncryptor { get; set; }

        /// <summary>
        /// Free all resources
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
            tcpClnt.Dispose();
        }

        /// <summary>
        /// Reads data from server
        /// </summary>
        /// <returns>Byte array of data received</returns>
        private byte[] readData()
        {
            var data = new byte[Packet.MaxPacketSize];
            int length = stream.Read(data, 0, Packet.MaxPacketSize);
            if (length == 0)
                return null;

            return data.Take(length).ToArray();
        }
    }
}
