namespace Matias_ToDo_DoubleDB.Code.Services;
public interface IHashingService
{
    public object MD5Hash(string input, bool returnAsString = true);
    public string SHA256Hash(string input);
    public string HMCHashing(string input);
    public string Pbkdf2Hashing(string input);
    public string BCryptHashing(string input);
    public bool BCryptHashValidator(string input, string hashedValue);
}
