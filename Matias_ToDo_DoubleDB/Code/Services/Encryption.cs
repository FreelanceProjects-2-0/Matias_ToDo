using System.Security.Cryptography;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class Encryption
    {

        public static string Encrypter(string textToEncrypt, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(publicKey);

                byte[] byteArrayInputText = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                byte[] encryptedTextByteArray = rsa.Encrypt(byteArrayInputText, true);

                return Convert.ToBase64String(encryptedTextByteArray);
            }
        }
    }
}
