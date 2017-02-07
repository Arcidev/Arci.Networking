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
        public void Test16ByteAesEncryptor()
        {
            TestAesEncryptorCreation(16);
        }

        [TestMethod]
        public void Test32ByteAesEncryptor()
        {
            TestAesEncryptorCreation(32);
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

        [TestMethod]
        public void TestNonePadding()
        {
            using (var aes = new AesEncryptor() { PaddingMode = PaddingMode.None })
            {
                var sb = new StringBuilder("Hello from unecrypted world");
                if(sb.Length % aes.CurrentKeyByteLength != 0)
                    sb.Append('\0', aes.CurrentKeyByteLength - sb.Length % aes.CurrentKeyByteLength);

                var value = sb.ToString();
                var encryptedVal = aes.Encrypt(value);
                Assert.AreNotEqual(value, encryptedVal, "Value is not encrypted");

                // No trim
                var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal));
                Assert.AreEqual(value, decryptedVal, $"Value is not the same as before encryption");

                // Trim added zeroes
                decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
                Assert.AreEqual(value.TrimEnd('\0'), decryptedVal, $"Value is not the same as before encryption");
            }
        }

        private void TestEncryption(PaddingMode padding)
        {
            using (var aes = new AesEncryptor() { PaddingMode = padding })
            {
                var value = "Hello from unecrypted world";
                var encryptedVal = aes.Encrypt(value);
                Assert.AreNotEqual(value, encryptedVal, "Value is not encrypted");

                // We need to trim \0 char from string as Aes ZeroesPadding is adding zeroes but not removing them
                var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
                Assert.AreEqual(value, decryptedVal, $"Value is not the same as before encryption");
            }
        }

        private void TestAesEncryptorCreation(int keyLength)
        {
            var aesKey = new byte[keyLength];
            var iVec = new byte[16];
            for (byte i = 0; i < keyLength; i++)
            {
                aesKey[i] = i;
                if (keyLength < 16)
                    iVec[i] = (byte)(byte.MaxValue - i);
            }

            using (AesEncryptor aes = new AesEncryptor(keyLength), aes2 = new AesEncryptor(aesKey, iVec))
            {
                Assert.AreNotEqual(null, aes.Encryptors, "Encryptors not created");
                Assert.AreEqual(aes.Encryptors.Length, keyLength + iVec.Length, "Invalid length of encryptors");

                Assert.AreNotEqual(null, aes2.Encryptors, "Encryptors not created");
                Assert.IsTrue(aesKey.Concat(iVec).SequenceEqual(aes2.Encryptors), "Encryptors not created correctly");
            }
        }
    }
}
