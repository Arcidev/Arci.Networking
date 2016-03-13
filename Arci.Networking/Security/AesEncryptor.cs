using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System;

namespace Arci.Networking.Security
{
    /// <summary>
    /// Aes encryptor
    /// </summary>
    public class AesEncryptor : IDisposable
    {
        private Aes aes;
        private byte[] iVec = new byte[16];
        private byte[] key = new byte[16];

        /// <summary>
        /// Current encryptors. First 16 bytes represent key, the rest 16 bytes represent iVec
        /// </summary>
        public byte[] Encryptors { get { return key.Concat(iVec).ToArray(); } }

        /// <summary>
        /// Creates AES instance
        /// </summary>
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

        /// <summary>
        /// Creates AES instance
        /// </summary>
        /// <param name="key">Key to be set as AES key</param>
        /// <param name="iVec">IVec to be set as AES iVec</param>
        public AesEncryptor(byte[] key, byte[] iVec)
        {
            this.key = key;
            this.iVec = iVec;

            aes = Aes.Create();
            aes.IV = iVec;
            aes.Key = key;
            aes.Padding = PaddingMode.Zeros;
        }

        /// <summary>
        /// Encrypts data with current encryptor
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(string toEncrypt)
        {
            return Encrypt(System.Text.Encoding.ASCII.GetBytes(toEncrypt));
        }

        /// <summary>
        /// Encrypts data
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
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

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecrypt">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(string toDecrypt)
        {
            return Decrypt(System.Text.Encoding.ASCII.GetBytes(toDecrypt));
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecode">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
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

        /// <summary>
        /// Clears allocated resources
        /// </summary>
        public void Dispose()
        {
            aes.Clear();
        }
    }
}
