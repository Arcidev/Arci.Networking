using Arci.Networking.Data;
using Arci.Networking.Security;
using Arci.Networking.Security.AesOptions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Arci.Networking.Tests.NetCore.EncryptionTests
{
    public class AesTests
    {
        [Fact]
        public void Test128AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes128Bits);
        }

        [Fact]
        public void Test192AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes192Bits);
        }

        [Fact]
        public void Test256AesEncryptor()
        {
            TestAesEncryptorCreation(AesEncryptionType.Aes256Bits);
        }

        [Fact]
        public void TestZeroesPadding()
        {
            TestEncryption(PaddingMode.Zeros);
        }

        [Fact]
        public void TestPKCS7Padding()
        {
            TestEncryption(PaddingMode.PKCS7);
        }

        [Fact]
        public void TestPacketEncryption()
        {
            var packet = new Packet(0).Builder().WriteBit(true).WriteBit(2).FlushBits().Write(new byte[] { 1, 2, 3 }).Build();
            using (var aes = new AesEncryptor() { PaddingMode = PaddingMode.PKCS7 })
            {
                var decrypted = aes.Decrypt(aes.Encrypt(packet.Data));
                Assert.Equal<byte>(packet.Data, decrypted);
            }
        }

        [Fact]
        public void TestNonePadding()
        {
            using (var aes = new AesEncryptor() { PaddingMode = PaddingMode.None })
            {
                var sb = new StringBuilder("Hello from unecrypted world");
                if(sb.Length % aes.Key.Length != 0)
                    sb.Append('\0', aes.Key.Length - sb.Length % aes.Key.Length);

                var value = sb.ToString();
                var encryptedVal = aes.Encrypt(value);
                Assert.NotEqual<byte>(Encoding.ASCII.GetBytes(value), encryptedVal);

                // No trim
                var decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII);
                Assert.Equal(value, decryptedVal);

                // Trim added zeroes
                decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII).TrimEnd('\0');
                Assert.Equal(value.TrimEnd('\0'), decryptedVal);
            }
        }

        [Fact]
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

                    Assert.NotEqual(keyList[i], key[i]);
                    Assert.Equal(keyList[i], aes.Key[i]);

                    Assert.NotEqual(ivList[i], iv[i]);
                    Assert.Equal(ivList[i], aes.IVec[i]);
                }
            }
        }

        private void TestEncryption(PaddingMode padding)
        {
            using (var aes = new AesEncryptor(AesEncryptionType.Aes256Bits) { PaddingMode = padding })
            {
                var value = "Hello from unecrypted world";
                var encryptedVal = aes.Encrypt(value);
                Assert.NotEqual<byte>(Encoding.ASCII.GetBytes(value), encryptedVal);

                // We need to trim \0 char from string as Aes ZeroesPadding is adding zeroes but not removing them
                var decryptedVal = aes.Decrypt(encryptedVal, Encoding.ASCII).TrimEnd('\0');
                Assert.Equal(value, decryptedVal);
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
                Assert.NotEqual(null, aes.Encryptors);
                Assert.Equal(aes.Encryptors.Length, (int)type + iVec.Length);

                Assert.NotEqual(null, aes2.Encryptors);
                Assert.Equal(aesKey.Concat(iVec), aes2.Encryptors);
            }
        }
    }
}
