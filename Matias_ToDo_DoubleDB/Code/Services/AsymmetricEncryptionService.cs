using System;
using System.Security.Cryptography;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class AsymmetricEncryptionService : IAsymmetricEncryptionService
    {
        public string? _privateKey { get; private set; }
        public string? _publicKey { get; private set; }

        private string userPrivateKey = string.Empty;
        private string userPublicKey = string.Empty;


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

        public bool UpdateKeys(string privateKey, string publicKey)
        {
            userPrivateKey = privateKey;
            userPublicKey = publicKey;
            return true;
        }
        public string EncryptAsymmetric(string inputText)
        {
            return Encryption.Encrypter(inputText, userPublicKey);
        }

        public string DecryptAsymmetric(string inputText)
        {
            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(userPrivateKey);

                byte[] byteArrayInputText = Convert.FromBase64String(inputText);

                byte[] decryptedTextByteArray = rsa.Decrypt(byteArrayInputText, true);

                return System.Text.Encoding.UTF8.GetString(decryptedTextByteArray);
            }
            //}

            //public string EncryptAsymmetric(string inputText, string publicKey)
            //{
            //    return Encryption.Encrypter(inputText, publicKey);
            //}

            //public string DecryptAsymmetric(string inputText, string privateKey)
            //{
            //    using (RSACryptoServiceProvider rsa = new())
            //    {
            //        rsa.FromXmlString(privateKey);

            //        byte[] byteArrayInputText = Convert.FromBase64String(inputText);

            //        byte[] decryptedTextByteArray = rsa.Decrypt(byteArrayInputText, true);

            //        return System.Text.Encoding.UTF8.GetString(decryptedTextByteArray);
            //    }
        }
    }
}