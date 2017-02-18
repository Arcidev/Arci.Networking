﻿using Arci.Networking.Security;
using Arci.Networking.Security.AesOptions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Arci.Networking.Tests.EncryptionTests
{
    [TestClass]
    public class AesTests
    {
        [TestMethod]
        public void Test128AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes128Bits);
        }

        [TestMethod]
        public void Test192AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes192Bits);
        }

        [TestMethod]
        public void Test256AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes256Bits);
        }

        [TestMethod]
        public void TestZeroesPadding()
        {
            TestEncryption(PaddingMode.Zeros);
        }

        [TestMethod]
        public void TestPKCS7Padding()
        {
            TestEncryption(PaddingMode.PKCS7);
        }

        [TestMethod]
        public void TestNonePadding()
        {
            using (var aes = new AesEncryptor() { PaddingMode = PaddingMode.None })
            {
                var sb = new StringBuilder("Hello from unecrypted world");
                if(sb.Length % aes.Key.Length != 0)
                    sb.Append('\0', aes.Key.Length - sb.Length % aes.Key.Length);

                var value = sb.ToString();
                var encryptedVal = aes.Encrypt(value);
                Assert.AreNotEqual(Encoding.ASCII.GetBytes(value), encryptedVal, "Value is not encrypted");

                // No trim
                var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal));
                Assert.AreEqual(value, decryptedVal, "Value is not the same as before encryption");

                // Trim added zeroes
                decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
                Assert.AreEqual(value.TrimEnd('\0'), decryptedVal, "Value is not the same as before encryption");
            }
        }

        private void TestEncryption(PaddingMode padding)
        {
            using (var aes = new AesEncryptor(AesEncryptionType.Aes256Bits) { PaddingMode = padding })
            {
                var value = "Hello from unecrypted world";
                var encryptedVal = aes.Encrypt(value);
                Assert.AreNotEqual(Encoding.ASCII.GetBytes(value), encryptedVal, "Value is not encrypted");

                // We need to trim \0 char from string as Aes ZeroesPadding is adding zeroes but not removing them
                var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
                Assert.AreEqual(value, decryptedVal, "Value is not the same as before encryption");
            }
        }

        private void TestAesEncryptorCreation(AesEncryptionType type)
        {
            var aesKey = new byte[(int)type];
            var iVec = new byte[16];
            for (byte i = 0; i < (int)type; i++)
            {
                aesKey[i] = i;
                if (i < 16)
                    iVec[i] = (byte)(byte.MaxValue - i);
            }

            using (AesEncryptor aes = new AesEncryptor(type), aes2 = new AesEncryptor(aesKey, iVec))
            {
                Assert.AreNotEqual(null, aes.Encryptors, "Encryptors not created");
                Assert.AreEqual(aes.Encryptors.Length, (int)type + iVec.Length, "Invalid length of encryptors");

                Assert.AreNotEqual(null, aes2.Encryptors, "Encryptors not created");
                Assert.IsTrue(aesKey.Concat(iVec).SequenceEqual(aes2.Encryptors), "Encryptors not created correctly");
            }
        }
    }
}
