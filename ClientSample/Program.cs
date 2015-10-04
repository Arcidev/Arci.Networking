﻿using Arci.Networking;
using Arci.Networking.Data;
using Arci.Networking.Security;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = Client.Create("localhost", 10751);
            if (client == null)
            {
                Console.WriteLine("Server is offline");
                return;
            }

            // Sending AES key via RSA encryption
            // Aes (key, ivec generation)
            AesEncryptor aes = new AesEncryptor();
            // Rsa (sets server public key)
            RsaEncryptor rsa = new RsaEncryptor(RSAKey.Modulus, RSAKey.PublicExponent);
            // Sets AES key for our client, key will be used for SendPacket function and ReceiveData function
            client.AesEncryptor = aes;

            // Creating new packet instance
            Packet packet = new Packet(ClientPacketTypes.CMSG_INIT_ENCRYPTED_RSA);
            packet.Write(rsa.Encrypt(aes.Encryptors));
            client.SendPacket(packet, false);
            // We will not need this anymore
            rsa.Dispose();

            // Sending fully encrypted packet with AES
            // Reinicialize data inside of packet (Dispose is called also)
            packet.Initialize(ClientPacketTypes.CMSG_INIT_ENCRYPTED_AES);
            packet.Write("And Once More Hello to You from Fully Encrypted Packet!");
            client.SendPacket(packet);
            packet.Dispose();

            Thread.Sleep(3000);
            IEnumerable<Packet> packets = null;
            do
            {
                packets = client.ReceiveData(true);
                if (packets == null)
                    break;

                foreach(var pck in packets)
                {
                    switch (pck.OpcodeNumber)
                    {
                        case (UInt16)ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_RSA:
                            Console.WriteLine(pck.ReadString());
                            break;
                        case (UInt16)ServerPacketTypes.SMSG_INIT_RESPONSE_ENCRYPTED_AES:
                            Console.WriteLine(pck.ReadString());
                            break;
                    }

                    pck.Dispose();
                }
            } while (true);

            // CleanUP at the end
            aes.Dispose();
            client.Dispose();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
