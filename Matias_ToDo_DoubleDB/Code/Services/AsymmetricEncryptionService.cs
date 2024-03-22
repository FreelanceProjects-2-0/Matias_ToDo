using System;
using System.Security.Cryptography;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class AsymmetricEncryptionService : IAsymmetricEncryptionService
    {
        public string? _privateKey { get; private set; }
        public string? _publicKey { get; private set; }


        public AsymmetricEncryptionService()
        {
            using (RSA rsa = RSA.Create())
            {
                RSAParameters privateKeyParameters = rsa.ExportParameters(true);
                RSAParameters publicKeyParameters = rsa.ExportParameters(false);

                _privateKey = rsa.ToXmlString(true);
                _publicKey = rsa.ToXmlString(false);
            }
        }

        public string EncryptAsymmetric(string inputText, string publicKey)
        {
            return Encryption.Encrypter(inputText, publicKey);
        }

        public string DecryptAsymmetric(string inputText, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(privateKey);

                byte[] byteArrayInputText = Convert.FromBase64String(inputText);

                byte[] decryptedTextByteArray = rsa.Decrypt(byteArrayInputText, true);

                return System.Text.Encoding.UTF8.GetString(decryptedTextByteArray);
            }
        }
    }
}