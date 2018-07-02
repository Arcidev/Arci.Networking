using Arci.Networking.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System.Text;

namespace Arci.Networking.Tests.EncryptionTests
{
    [TestClass]
    public class RsaTests
    {
        [TestMethod]
        public void TestPkcsPadding()
        {
            TestEncryption(false);
        }

        [TestMethod]
        public void TestOEAPPadding()
        {
            TestEncryption(true);
        }

        private void TestEncryption(bool useOaepPadding)
        {
            using (RsaEncryptor rsa = new RsaEncryptor(RSAKey.Modulus, RSAKey.PublicExponent) { UseOAEPPadding = useOaepPadding },
                rsa2 = new RsaEncryptor(RSAKey.RsaParams) { UseOAEPPadding = useOaepPadding })
            {
                var value = Encoding.ASCII.GetBytes("Hello from unecrypted world");
                var rsaEncryptedValue = rsa.Encrypt(value);
                var rsa2EncryptedValue = rsa2.Encrypt(value);

                // Variable rsa is used only for encryption so we are not going to use it for decryption
                // Instead We decrypt both values with rsa2 variable
                var rsaDecryptedValue = rsa2.Decrypt(rsaEncryptedValue);
                var rsa2DecryptedValue = rsa2.Decrypt(rsa2EncryptedValue);

                CollectionAssert.AreEqual(rsaDecryptedValue, rsa2DecryptedValue);
                CollectionAssert.AreEqual(value, rsaDecryptedValue);
            }
        }
    }
}
