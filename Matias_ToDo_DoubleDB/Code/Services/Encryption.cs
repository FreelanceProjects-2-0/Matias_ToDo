using System.Security.Cryptography;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class Encryption
    {

        public static string Encrypter(string textToEncrypt, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);

                byte[] byteArrayInputText = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                byte[] encryptedTextByteArray = rsa.Encrypt(byteArrayInputText, RSAEncryptionPadding.OaepSHA256);

                return Convert.ToBase64String(encryptedTextByteArray);
            }
        }
    }
}
