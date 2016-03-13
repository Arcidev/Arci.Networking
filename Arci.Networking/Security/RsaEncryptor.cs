using System;
using System.Security.Cryptography;

namespace Arci.Networking.Security
{
    /// <summary>
    /// Rsa encryptor
    /// </summary>
    public class RsaEncryptor : IDisposable
    {
        private RSACryptoServiceProvider rsa;

        /// <summary>
        /// Creates RSA instance. Used only for encryption
        /// </summary>
        /// <param name="modulus">Modulus to be set</param>
        /// <param name="publicExponent">Exponent to be set</param>
        public RsaEncryptor(byte[] modulus, byte[] publicExponent)
        {
            rsa = new RSACryptoServiceProvider();

            RSAParameters rsaParams = new RSAParameters();
            rsaParams.Exponent = publicExponent;
            rsaParams.Modulus = modulus;
            rsa.ImportParameters(rsaParams);
        }

        /// <summary>
        /// Creates RSA instance
        /// </summary>
        /// <param name="rsaParams">Custom RSA Parameters</param>
        public RsaEncryptor(RSAParameters rsaParams)
        {
            rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParams);
        }

        /// <summary>
        /// Encrypts data
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(byte[] toEncrypt)
        {
            return rsa.Encrypt(toEncrypt, false);
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecrypt">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(byte[] toDecrypt)
        {
            return rsa.Decrypt(toDecrypt, false);
        }

        /// <summary>
        /// Clears allocated resources
        /// </summary>
        public void Dispose()
        {
            rsa.Clear();
        }
    }
}
