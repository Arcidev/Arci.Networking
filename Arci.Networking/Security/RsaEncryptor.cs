using System;
using System.Security.Cryptography;

namespace Arci.Networking.Security
{
    public class RsaEncryptor : IDisposable
    {
        private RSACryptoServiceProvider rsa;

        // Creates RSA instance
        public RsaEncryptor(byte[] modulus, byte[] publicExponent)
        {
            rsa = new RSACryptoServiceProvider();

            var s = rsa.ExportParameters(true);
            RSAParameters rsaParams = new RSAParameters();
            rsaParams.Exponent = publicExponent;
            rsaParams.Modulus = modulus;
            rsa.ImportParameters(rsaParams);
        }

        // Creates RSA instance
        public RsaEncryptor(RSAParameters rsaParams)
        {
            rsa = new RSACryptoServiceProvider();
            var s = rsa.ExportParameters(true);

            rsa.ImportParameters(rsaParams);
        }

        // Encrypts data
        public byte[] Encrypt(byte[] toEncrypt)
        {
            return rsa.Encrypt(toEncrypt, false);
        }

        // Decrypts data
        public byte[] Decrypt(byte[] toDecrypt)
        {
            return rsa.Decrypt(toDecrypt, false);
        }

        // Clears allocated resources
        public void Dispose()
        {
            rsa.Clear();
        }
    }
}
