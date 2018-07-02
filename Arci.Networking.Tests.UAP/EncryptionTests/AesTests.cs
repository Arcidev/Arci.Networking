using Arci.Networking.Data;
using Arci.Networking.Security;
using Arci.Networking.Security.AesOptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Arci.Networking.Tests.UAP.EncryptionTests
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
        public void TestPacketEncryption()
        {
            var packet = new Packet(0).Builder().WriteBit(true).WriteBit(2).FlushBits().Write(new byte[] { 1, 2, 3 }).Build();
            using (var aes = new AesEncryptor() { PaddingMode = PaddingMode.PKCS7 })
            {
                var decrypted = aes.Decrypt(aes.Encrypt(packet.Data));
                CollectionAssert.AreEqual(packet.Data, decrypted);
            }
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
                Assert.IsFalse(Encoding.ASCII.GetBytes(value).SequenceEqual(encryptedVal), "Value is not encrypted");
                CollectionAssert.AreNotEqual(Encoding.ASCII.GetBytes(value), encryptedVal);

                // No trim
                var decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII);
                Assert.AreEqual(value, decryptedVal, "Value is not the same as before encryption");

                // Trim added zeroes
                decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII).TrimEnd('\0');
                Assert.AreEqual(value.TrimEnd('\0'), decryptedVal, "Value is not the same as before encryption");
            }
        }

        [TestMethod]
        public void TestKeyImmutability()
        {
            using (var aes = new AesEncryptor(AesEncryptionType.Aes128Bits))
            {
                var key = aes.Key;
                var iv = aes.IVec;

                var keyList = key.ToList();
                var ivList = iv.ToList();

                for (var i = 0; i < (int)AesEncryptionType.Aes128Bits; i++)
                {
                    key[i] = (byte)((key[i] + 1) % byte.MaxValue);
                    iv[i] = (byte)((iv[i] + 1) % byte.MaxValue);

                    Assert.AreNotEqual(keyList[i], key[i]);
                    Assert.AreEqual(keyList[i], aes.Key[i]);

                    Assert.AreNotEqual(ivList[i], iv[i]);
                    Assert.AreEqual(ivList[i], aes.IVec[i]);
                }
            }
        }

        private void TestEncryption(PaddingMode padding)
        {
            using (var aes = new AesEncryptor(AesEncryptionType.Aes256Bits) { PaddingMode = padding })
            {
                var value = "Hello from unecrypted world";
                var encryptedVal = aes.Encrypt(value);
                CollectionAssert.AreNotEqual(Encoding.ASCII.GetBytes(value), encryptedVal);

                // We need to trim \0 char from string as Aes ZeroesPadding is adding zeroes but not removing them
                var decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII).TrimEnd('\0');
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
                CollectionAssert.AreEqual(aesKey.Concat(iVec).ToList(), aes2.Encryptors);
            }
        }
    }
}
