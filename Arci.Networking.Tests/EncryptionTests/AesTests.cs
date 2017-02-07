using Arci.Networking.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Arci.Networking.Tests.EncryptionTests
{
    [TestClass]
    public class AesTests
    {
        [TestMethod]
        public void TestAesEncryptorCreation()
        {
            var aesKey = new byte[16];
            var iVec = new byte[16];
            for (byte i = 0; i < 16; i++)
            {
                aesKey[i] = i;
                iVec[i] = (byte)(byte.MaxValue - i);
            }

            using (AesEncryptor aes = new AesEncryptor(), aes2 = new AesEncryptor(aesKey, iVec))
            {
                Assert.AreNotEqual(null, aes.Encryptors, "Encryptors not created");
                Assert.AreEqual(aes.Encryptors.Length, 32, "Invalid length of encryptors");

                Assert.AreNotEqual(null, aes2.Encryptors, "Encryptors not created");
                Assert.IsTrue(aesKey.Concat(iVec).SequenceEqual(aes2.Encryptors), "Encryptors not created correctly");
            }
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
        public void TestANSIX923Padding()
        {
            TestEncryption(PaddingMode.ANSIX923);
        }

        [TestMethod]
        public void TestISO10126Padding()
        {
            TestEncryption(PaddingMode.ISO10126);
        }

        private void TestEncryption(PaddingMode padding)
        {
            using (var aes = new AesEncryptor())
            {
                aes.SetPaddingMode(padding);

                var value = "Hello from unecrypted world";
                var encryptedVal = aes.Encrypt(value);
                Assert.AreNotEqual(value, encryptedVal, "Value is not encrypted");

                // We need to trim \0 char from string as Aes is always creating block of 16 bytes
                var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
                Assert.AreEqual(value, decryptedVal, $"Value is not the same as before encryption");
            }
        }
    }
}
