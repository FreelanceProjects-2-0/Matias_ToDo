using Matias_ToDo_DoubleDB.Data.Models.Entities;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public interface IToDoService
    {
        public Task<bool> AddTask(string listId, string title, string description);
        public Task<ToDoList> GetTasks(string email);
    }
}
