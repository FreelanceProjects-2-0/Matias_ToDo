namespace Matias_ToDo_DoubleDB.Code.Services
{
    public interface IAsymmetricEncryptionService
    {
        public string _privateKey { get; }
        public string _publicKey { get; }
        public string EncryptAsymmetric(string inputText);
        public string DecryptAsymmetric(string inputText);
        public bool UpdateKeys(string privateKey, string publicKey);
    }
}
