namespace Matias_ToDo_DoubleDB.Code.Services;

public interface ICprService
{
    public Task<bool> AddCprToUser(string cpr, string userEmail);
}