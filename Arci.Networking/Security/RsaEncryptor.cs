﻿using System;
using System.Security.Cryptography;

namespace Arci.Networking.Security
{
    /// <summary>
    /// Rsa encryptor
    /// </summary>
    public class RsaEncryptor : IDisposable
    {
        private readonly RSA rsa = RSA.Create();

        /// <summary>
        /// Sets true to use OAEP padding. Otherwise PKCS#1 v1.5 padding will be used
        /// </summary>
        public bool UseOAEPPadding { get; set; }

        /// <summary>
        /// Creates RSA instance. Used only for encryption
        /// </summary>
        /// <param name="modulus">Modulus to be set</param>
        /// <param name="publicExponent">Exponent to be set</param>
        public RsaEncryptor(byte[] modulus, byte[] publicExponent) : this(new RSAParameters { Exponent = publicExponent, Modulus = modulus }) { }

        /// <summary>
        /// Creates RSA instance
        /// </summary>
        /// <param name="rsaParams">Custom RSA Parameters</param>
        public RsaEncryptor(RSAParameters rsaParams)
        {
            rsa.ImportParameters(rsaParams);
        }

        /// <summary>
        /// Encrypts data
        /// </summary>
        /// <param name="toEncrypt">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(byte[] toEncrypt)
        {
            return rsa.Encrypt(toEncrypt, UseOAEPPadding ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        /// <param name="toDecrypt">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(byte[] toDecrypt)
        {
            return rsa.Decrypt(toDecrypt, UseOAEPPadding ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// Clears allocated resources
        /// </summary>
        public void Dispose()
        {
            rsa.Dispose();
        }
    }
}
