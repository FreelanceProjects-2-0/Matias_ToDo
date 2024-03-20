using System.Security.Cryptography;
using System.Text;

namespace Matias_ToDo_DoubleDB.Code.Services;
public class HashingService : IHashingService
{
    public object MD5Hash(string input, bool returnAsString = true)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);
        
        return returnAsString ? Convert.ToBase64String(hashBytes) : hashBytes;
    }

    public string SHA256Hash(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = SHA256.HashData(inputBytes);

        return Convert.ToBase64String(hashBytes);
    }
    public string HMCHashing(string input) => Convert.ToBase64String(new HMACSHA256(Encoding.ASCII.GetBytes("myKey")).ComputeHash(Encoding.ASCII.GetBytes(input)));

    //Password-Based Key Derivation Function 2
    public string Pbkdf2Hashing(string input)
    {
        byte[] salt = Encoding.ASCII.GetBytes("SaltySalt");
        HashAlgorithmName algorithm = new("SHA256");
        int outputLength = 32;
        int iterations = 10;
        byte[] hashedValue = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, outputLength);
        return Convert.ToBase64String(hashedValue);
    }

    public string BCryptHashing(string input)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        return BCrypt.Net.BCrypt.HashPassword(input, salt, true, BCrypt.Net.HashType.SHA256);


        //-----------------------------------------

        // 10 is iterations, 
        // true is to hash it wth sha-384 before returning the string value.

        //return BCrypt.Net.BCrypt.HashPassword(input, 10, true);

        //-----------------------------------------

        //return BCrypt.Net.BCrypt.HashPassword(input);
    }
    public bool BCryptHashValidator(string input, string hashedValue)
    {
        return BCrypt.Net.BCrypt.Verify(input, hashedValue, true, BCrypt.Net.HashType.SHA256);


        //-----------------------------------------

        //return BCrypt.Net.BCrypt.Verify(input, hashedValue, true);


        //-----------------------------------------
        
        //return BCrypt.Net.BCrypt.Verify(input, hashedValue);
    }
}
