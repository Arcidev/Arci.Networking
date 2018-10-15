using Arci.Networking;
using Arci.Networking.Data;
using Arci.Networking.Security;
using Arci.Networking.Security.AesOptions;
using Shared;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ClientSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Client client = await Client.CreateAsync("localhost", 10751);
            if (client == null)
            {
                Console.WriteLine("Server is offline");
                return;
            }

            // Sending AES key via RSA encryption
            // Aes (key, ivec generation)
            AesEncryptor aes = new AesEncryptor(AesEncryptionType.Aes256Bits) { PaddingMode = PaddingMode.PKCS7 };
            // Rsa (sets server public key)
            RsaEncryptor rsa = new RsaEncryptor(RSAKey.Modulus, RSAKey.PublicExponent) { UseOAEPPadding = true };
            // Sets AES key for our client, key will be used for SendPacket function and ReceiveData function
            client.Encryptor = aes;

            // Creating new packet instance
            Packet packet = new Packet(ClientPacketTypes.CMSG_INIT_ENCRYPTED_RSA);
            packet.Write(rsa.Encrypt(aes.Encryptors));
            client.SendPacket(packet, false);
            // We will not need this anymore
            rsa.Dispose();

            // Sending fully encrypted packet with AES
            // Reinicialize data inside of packet (Dispose is called also)
            packet.Initialize(ClientPacketTypes.CMSG_INIT_ENCRYPTED_AES);
            packet.Write("Hello Server from fully encrypted packet!");
            client.SendPacket(packet);
            packet.Dispose();

            bool end = false;
            while (!end)
            {
                var packets = await client.ReceiveDataAsync(true);
                foreach (var pck in packets)
                {
                    switch (pck.OpcodeNumber)
                    {
                        case (UInt16)ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_RSA:
                            Console.WriteLine(pck.ReadString());
                            break;
                        case (UInt16)ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_AES:
                            Console.WriteLine(pck.ReadString());
                            end = true;
                            break;
                    }

                    pck.Dispose();
                }
            }

            // CleanUP at the end
            aes.Dispose();
            client.Dispose();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
