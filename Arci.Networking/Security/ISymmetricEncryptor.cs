using System;

namespace Arci.Networking.Security
{
    /// <summary>
    /// Interface representing symmetric encryptor
    /// </summary>
    public interface ISymmetricEncryptor
    {
        /// <summary>
        /// Encrypts data with current encryptor
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        byte[] Encrypt(byte[] toEncrypt);

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecode">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        byte[] Decrypt(byte[] toDecode);
    }
}
