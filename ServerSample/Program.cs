using Arci.Networking;
using Arci.Networking.Data;
using Arci.Networking.Security;
using Shared;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace ServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer().Wait();
        }

        private static async Task RunServer()
        {
            // Client will send us the key and iVec
            AesEncryptor aes = null;
            // Rsa inicialization
            RsaEncryptor rsa = new RsaEncryptor(RSAKey.RsaParams) { UseOAEPPadding = true };

            // Creates new server instance on port 10751
            Server server = new Server(10751);
            // Awaits client connection
            var tcpClient = await server.AcceptClientAsync();
            // Creates client instance
            var client = new Client(tcpClient);
            do
            {
                var data = await client.ReceiveDataAsync();
                var end = false;
                do
                {
                    data = data.Any() ? data : await client.ReceiveDataAsync();
                    var length = BitConverter.ToUInt16(data, 0);
                    data = data.Skip(sizeof(UInt16)).ToArray();
                    var packetData = data.Take(length).ToArray();

                    // init packet, decrypt stream after inicialization
                    Packet packet = new Packet(aes != null ? aes.Decrypt(packetData) : packetData);
                    Packet response = null;
                    switch ((ClientPacketTypes)packet.OpcodeNumber)
                    {
                        case ClientPacketTypes.CMSG_INIT_ENCRYPTED_RSA:
                            var keys = rsa.Decrypt(packet.ReadBytes());
                            aes = new AesEncryptor(keys.Take(32).ToArray(), keys.Skip(32).ToArray()) { PaddingMode = PaddingMode.PKCS7 };
                            client.AesEncryptor = aes;
                            response = new Packet(ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_RSA);
                            response.Write("Hello Client!");
                            break;
                        case ClientPacketTypes.CMSG_INIT_ENCRYPTED_AES:
                            Console.WriteLine(packet.ReadString());
                            response = new Packet(ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_AES);
                            response.Write("Hello Client, We are now fully encrypted!");
                            end = true;
                            break;
                    }

                    if (response != null)
                    {
                        client.SendPacket(response);
                        response.Dispose();
                    }

                    data = data.Skip(length).ToArray();
                } while (!end);

                break;
            } while (true);

            Thread.Sleep(1000);
            aes.Dispose();
            client.Dispose();
            server.Dispose();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
