using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System;
using Arci.Networking.Security.AesOptions;
using System.Text;

namespace Arci.Networking.Security
{
    /// <summary>
    /// Aes encryptor
    /// </summary>
    public class AesEncryptor : IDisposable
    {
        private Aes aes;

        /// <summary>
        /// Returns copy of the current aes key
        /// </summary>
        public byte[] Key => aes.Key.ToArray();
        /// <summary>
        /// Returns copy of the current aes iVec
        /// </summary>
        public byte[] IVec => aes.IV.ToArray();
        /// <summary>
        /// Current encryptors. First bytes represent key, last 16 bytes represent iVec
        /// </summary>
        public byte[] Encryptors => aes.Key.Concat(aes.IV).ToArray();
        /// <summary>
        /// Aes padding mode to be used
        /// </summary>
        public PaddingMode PaddingMode
        {
            get { return aes.Padding; }
            set{ aes.Padding = value; }
        }

        /// <summary>
        /// Creates AES instance
        /// </summary>
        /// <param name="type">Bit version type of Aes to be used</param>
        public AesEncryptor(AesEncryptionType type = AesEncryptionType.Aes128Bits)
        {
            aes = Aes.Create();
            var iVec = new byte[16];
            var key = new byte[(int)type];

            using (var rng = RandomNumberGenerator.Create())
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
            aes = Aes.Create();
            aes.IV = iVec;
            aes.Key = key;
            aes.Padding = PaddingMode.Zeros;
        }

        /// <summary>
        /// Encrypts data with current encryptor
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(string toEncrypt, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            return Encrypt(encoding.GetBytes(toEncrypt));
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
        [Obsolete("Encrypted data are not suited to be in string form. Use Decrypt(byte[]) or Decrypt(byte[], Encoding) instead.")]
        public byte[] Decrypt(string toDecrypt)
        {
            return Decrypt(Encoding.ASCII.GetBytes(toDecrypt));
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecode">Data to decrypt</param>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        /// <returns>Decrypted data</returns>
        public string Decrypt(byte[] toDecode, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            return encoding.GetString(Decrypt(toDecode));
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
            aes.Dispose();
        }
    }
}
