using System;
using System.Linq;
using System.Net.Sockets;
using Arci.Networking.Data;
using Arci.Networking.Security;
using System.Collections.Generic;

namespace Arci.Networking
{
    public class Client : IDisposable
    {
        private TcpClient tcpClnt;
        private NetworkStream stream;

        public Client(TcpClient client)
        {
            tcpClnt = client;
            stream = tcpClnt.GetStream();
        }

        // Creates new network instance
        public static Client Create(string server, int port)
        {
            TcpClient client = null;

            // check connection
            try
            {
                client = new TcpClient(server, port);
            }
            catch (SocketException)
            {
                return null;
            }

            return new Client(client);
        }

        // Sends packet to server
        public void SendPacket(Packet packet, bool encrypt = true)
        {
            var data = encrypt && AesEncryptor != null ? AesEncryptor.Encrypt(packet.Data) : packet.Data;
            var toSend = BitConverter.GetBytes((UInt16)data.Length).Concat(data).ToArray();

            stream.Write(toSend, 0, data.Length + sizeof(UInt16));
        }

        // Receive data from server
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

        public byte[] ReceiveData()
        {
            if (stream.DataAvailable)
                return readData();

            return null;
        }

        public AesEncryptor AesEncryptor { get; set; }

        // Free all resources
        public void Dispose()
        {
            stream.Close();
            tcpClnt.Close();
        }

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
