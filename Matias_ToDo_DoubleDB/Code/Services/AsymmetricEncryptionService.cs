using System;
using System.Security.Cryptography;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class AsymmetricEncryptionService : IAsymmetricEncryptionService
    {
        private string pKey = "<RSAKeyValue><Modulus>2+zHsIEkbSYcH7rX2L/7xckWqf/5pKYHQAlmVp2g6RI+hsYBKTQ2KGsFVDfb7sc9ioypu3G4l9QQjPRGulkzxYFI6BMYS667MT7Vqih86+1d5i15lAfG600X14TuQKzTvvSD0h1pdmJf2ffnUz2m7csB4Y/tZm2fWRxLpdy4k6ko2KD3oYp3ckLbhfKpAQAmdO7thzjcx7JM7+08xs4iSPVBeQ58z7sYWHnvUfgLNAURmwR0kObHakcj33K1lyK9P99vvrYJGfGR8pUeopq5N6guCLWjG5naE5a18ZfPsa/GLel8AR5YuI64GAllTTJl1dffcAeKt5qxqsRIroKa1Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        
        public string? _privateKey { get; private set; }
        public string? _publicKey;


        public AsymmetricEncryptionService()
        {
            using (RSA rsa = RSA.Create())
            {
                RSAParameters privateKeyParameters = rsa.ExportParameters(true);

                _privateKey = rsa.ToXmlString(true);
                _publicKey = pKey;
            }
        }

        public string EncryptAsymmetric(string inputText)
        {
            return Encryption.Encrypter(inputText, _publicKey);
        }

        public string DecryptAsymmetric(string inputText, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);

                byte[] byteArrayInputText = Convert.FromBase64String(inputText);

                byte[] decryptedTextByteArray = rsa.Decrypt(byteArrayInputText, RSAEncryptionPadding.OaepSHA256);

                return System.Text.Encoding.UTF8.GetString(decryptedTextByteArray);
            }
        }
    }
}