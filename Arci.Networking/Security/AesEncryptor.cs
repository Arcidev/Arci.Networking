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
    public class AesEncryptor : ISymmetricEncryptor, IDisposable
    {
        private readonly Aes aes = Aes.Create();

        /// <summary>
        /// Returns copy of the current aes key
        /// </summary>
        public byte[] Key => aes.Key.ToArray();

        /// <summary>
        /// Returns copy of the current aes iVec
        /// </summary>
        public byte[] IVec => aes.IV.ToArray();

        /// <summary>
        /// Current encryptors. First 16/24/32 (based on <see cref="AesEncryptionType"/>) bytes represent key, last 16 bytes represent iVec
        /// </summary>
        public byte[] Encryptors => aes.Key.Concat(aes.IV).ToArray();

        /// <summary>
        /// Aes padding mode to be used
        /// </summary>
        public PaddingMode PaddingMode
        {
            get => aes.Padding;
            set => aes.Padding = value;
        }

        /// <summary>
        /// Creates AES instance
        /// </summary>
        /// <param name="type">Bit version type of Aes to be used</param>
        public AesEncryptor(AesEncryptionType type = AesEncryptionType.Aes128Bits)
        {
            var iVec = new byte[16];
            var key = new byte[(int)type];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
                rng.GetBytes(iVec);
            }

            aes.Key = key;
            aes.IV = iVec;
            aes.Padding = PaddingMode.Zeros;
        }

        /// <summary>
        /// Creates AES instance
        /// </summary>
        /// <param name="key">Key to be set as AES key</param>
        /// <param name="iVec">IVec to be set as AES iVec</param>
        public AesEncryptor(byte[] key, byte[] iVec)
        {
            aes.Key = key;
            aes.IV = iVec;
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
            return Encrypt((encoding ?? Encoding.ASCII).GetBytes(toEncrypt));
        }

        /// <summary>
        /// Encrypts data
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(byte[] toEncrypt)
        {
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new BinaryWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(toEncrypt);
                    }
                }

                return msEncrypt.ToArray();
            }
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecode">Data to decrypt</param>
        /// <param name="encoding">Encoding type of string. If null provided then ASCII will be used</param>
        /// <returns>Decrypted data</returns>
        public string Decrypt(byte[] toDecode, Encoding encoding)
        {
            return (encoding ?? Encoding.ASCII).GetString(Decrypt(toDecode));
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecode">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(byte[] toDecode)
        {
            using (var msDecrypt = new MemoryStream(toDecode, false))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new BinaryReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        return srDecrypt.ReadBytes(toDecode.Length);
                    }
                }
            }
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
