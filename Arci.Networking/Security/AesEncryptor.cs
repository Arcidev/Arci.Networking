using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System;

namespace Arci.Networking.Security
{
    public class AesEncryptor : IDisposable
    {
        private Aes aes;
        private byte[] iVec = new byte[16];
        private byte[] key = new byte[16];

        public byte[] Encryptors { get { return key.Concat(iVec).ToArray(); } }

        // Creates AES instance
        public AesEncryptor()
        {
            aes = Aes.Create();
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
                rng.GetBytes(iVec);
            }

            aes.IV = iVec;
            aes.Key = key;
            aes.Padding = PaddingMode.Zeros;
        }

        public AesEncryptor(byte[] key, byte[] iVec)
        {
            aes = Aes.Create();
            aes.IV = iVec;
            aes.Key = key;
            aes.Padding = PaddingMode.Zeros;
        }

        // Encrypts data
        public byte[] Encrypt(string toEncrypt)
        {
            return Encrypt(System.Text.Encoding.ASCII.GetBytes(toEncrypt));
        }

        // Encrypts data
        public byte[] Encrypt(byte[] toEncrypt)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encodedText = null;

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(toEncrypt);
                    }

                    encodedText = msEncrypt.ToArray();
                }
            }

            return encodedText;
        }

        // Decrypts data
        public byte[] Decrypt(string toDecrypt)
        {
            return Decrypt(System.Text.Encoding.ASCII.GetBytes(toDecrypt));
        }

        // Decrypts data
        public byte[] Decrypt(byte[] toDecode)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decodedText = null;

            using (MemoryStream msDecrypt = new MemoryStream(toDecode.ToArray()))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (BinaryReader srDecrypt = new BinaryReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        decodedText = srDecrypt.ReadBytes(toDecode.Length);
                    }
                }
            }

            return decodedText;
        }

        // Clears allocated resources
        public void Dispose()
        {
            aes.Clear();
        }
    }
}
