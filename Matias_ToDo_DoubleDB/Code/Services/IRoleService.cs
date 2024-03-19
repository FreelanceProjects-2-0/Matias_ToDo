namespace Matias_ToDo_DoubleDB.Code.Services;
public interface IRoleService
{
    public Task<bool> CreateUserRole(string role, IServiceProvider serviceProvider, string userEmail);
}