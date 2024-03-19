using Matias_ToDo_DoubleDB.Data;
using Matias_ToDo_DoubleDB.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
//using IHttpContextAccessor httpContextAccessor

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IRoleService _roleService;
        private readonly DataDBContext _dataDbContext;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        UserManager<ApplicationUser> _userManager;
        IServiceProvider _serviceProvider;
        public ToDoService(DataDBContext dataDbContext, IRoleService roleService, ILogger<ToDoService> logger, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _dataDbContext = dataDbContext;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddTask(string listId, string title, string description)
        {
            _logger.LogTrace($"Adding task {title} to todo-list with id: {listId}.");

            string? userEmail = _httpContextAccessor?.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail)) throw new Exception($"Current user not found in Database {userEmail}");

            Guid userGuid = Guid.Parse((await _userManager.FindByEmailAsync(userEmail.ToUpper()))!.Id);

            ToDoItem item = new() { Title = title, Description = description };

            ToDoList todoList = await NullOrEmptyList(userEmail, userGuid, listId);
            todoList.Items.Add(item);

            return await Save();
        }

        public async Task<ToDoList> GetTasks(string userEmail)
        {
            Guid userGuid = Guid.Parse((await _userManager.FindByEmailAsync(userEmail.ToUpper()))!.Id);
            ToDoList? todoList = null;
            try
            {
                todoList = await NullOrEmptyList(userEmail, userGuid, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in ToDoService. GetTasks see exception {ex}");
            }

            return todoList;
        }
        public async Task<bool> Save()
        {
            return await _dataDbContext.SaveChangesAsync() > 0;
        }

        public async Task<ToDoList> NullOrEmptyList(string userEmail, Guid userGuid, string listId) => listId != null
                ? await _dataDbContext.ToDoLists.FirstOrDefaultAsync(x =>
                    x.Id == Guid.Parse(listId) && x.IdentityId == userGuid)
                    ?? throw new Exception($"No todo-list found with Id: {listId} for user with email {userEmail}")
                : await _dataDbContext.ToDoLists.FirstOrDefaultAsync(x =>
                    x.IdentityId == userGuid)
                    ?? throw new Exception($"No todo-list found with Id: {listId} for user with email {userEmail}");



        //public async Task<ToDoList> NullOrEmptyList2(string userEmail, Guid userGuid, string listId)
        //{
        //    //ToDoList? toDoList = null;

        //    return listId != null 
        //        ? await _dataDbContext.ToDoLists.FirstOrDefaultAsync(x => 
        //            x.Id == Guid.Parse(listId) && x.IdentityId == userGuid) 
        //            ?? throw new Exception($"No todo-list found with Id: {listId} for user with email {userEmail}") 
        //        : await _dataDbContext.ToDoLists.FirstOrDefaultAsync(x => 
        //            x.IdentityId == userGuid) 
        //            ?? throw new Exception($"No todo-list found with Id: {listId} for user with email {userEmail}");
        //}
    }
}
