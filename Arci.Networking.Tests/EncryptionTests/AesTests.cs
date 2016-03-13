using Arci.Networking.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Arci.Networking.Tests.EncryptionTests
{
    [TestClass]
    public class AesTests
    {
        [TestMethod]
        public void TestAesEncryptorCreation()
        {
            var aes = new AesEncryptor();
            Assert.AreNotEqual(null, aes.Encryptors, "Encryptors not created");
            Assert.AreEqual(aes.Encryptors.Length, 32, "Invalid length of encryptors");

            var aesKey = new byte[16];
            var iVec = new byte[16];
            for (byte i = 0; i < 16; i++)
            {
                aesKey[i] = i;
                iVec[i] = (byte)(byte.MaxValue - i);
            }

            var aes2 = new AesEncryptor(aesKey, iVec);
            Assert.AreNotEqual(null, aes2.Encryptors, "Encryptors not created");
            Assert.IsTrue(aesKey.Concat(iVec).SequenceEqual(aes2.Encryptors), "Encryptors not created correctly");

            aes.Dispose();
            aes2.Dispose();
        }

        [TestMethod]
        public void TestEncryption()
        {
            var value = "Hello from unecrypted world";
            var aes = new AesEncryptor();

            var encryptedVal = aes.Encrypt(value);
            Assert.AreNotEqual(value, encryptedVal, "Value is not encrypted");

            // We need to trim \0 char from string as Aes is always creating block of 16 bytes
            var decryptedVal = Encoding.ASCII.GetString(aes.Decrypt(encryptedVal)).TrimEnd('\0');
            Assert.AreEqual(value, decryptedVal, "Value is not the same as before encryption");

            aes.Dispose();
        }
    }
}
