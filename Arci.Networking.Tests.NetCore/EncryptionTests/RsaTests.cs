using Arci.Networking.Security;
using Shared;
using System.Text;
using Xunit;

namespace Arci.Networking.Tests.NetCore.EncryptionTests
{
    public class RsaTests
    {
        [Fact]
        public void TestPkcsPadding()
        {
            TestEncryption(false);
        }

        [Fact]
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

                Assert.Equal<byte>(rsaDecryptedValue, rsa2DecryptedValue);
                Assert.Equal<byte>(value, rsaDecryptedValue);
            }
        }
    }
}
