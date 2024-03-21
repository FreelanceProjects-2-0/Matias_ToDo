using Matias_ToDo_DoubleDB.Data.Models.Entities;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public interface IToDoService
    {
        public Task<bool> AddTask(string userEmail, string title);
        public Task<List<ToDoItem>> GetTasks(string userEmail);
        public Task<bool> RemoveItem(string userEmail, Guid itemId);
        public Task<bool> AdminDeleteAllTasks(string userEmail);
    }
}
