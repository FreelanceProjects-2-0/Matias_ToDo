using Matias_ToDo_DoubleDB.Data;
using Matias_ToDo_DoubleDB.Data.Models.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Matias_ToDo_DoubleDB.Code.Services
{
    public class ToDoService : IToDoService
    {
        private readonly DataDBContext _dbContext;
        private readonly ApplicationDbContext _context;
        private readonly IAsymmetricEncryptionService _encryptionService;

        public ToDoService(DataDBContext dataDBContext, ApplicationDbContext context, IAsymmetricEncryptionService asymmetricEncryptionService)
        {
            _dbContext = dataDBContext;
            _context = context;
            _encryptionService = asymmetricEncryptionService;
        }

        public async Task<bool> AddTask(string userEmail, string title)
        {
            string userId = _context.Users
                .Where(x => x.NormalizedEmail == userEmail.ToUpper())
                .FirstOrDefault().Id;

            if (userId == null) throw new Exception($"User with email {userEmail} not found.");

            var publicKey = await _dbContext.Cprs
                    .Where(x => x.IdentityId == Guid.Parse(userId))
                    .Select(z => z.PublicKey)
                    .FirstOrDefaultAsync();
            if (publicKey == null) throw new Exception($"No public key found for user id: {userId}");

            //title = _encryptionService.EncryptAsymmetric(title);

            var item = new ToDoItem() { Title = title, IdentityId = Guid.Parse(userId) };
            await _dbContext.AddAsync(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<ToDoItem>> GetTasks(string userEmail)
        {
            try
            {

                List<ToDoItem> encryptedTasks = new();
                string userId = _context.Users
                    .Where(x => x.NormalizedEmail == userEmail.ToUpper())
                    .FirstOrDefault().Id;

                if (userId == null) throw new Exception($"User with email {userEmail} not found.");

                string? userPrivateKey = await _dbContext.Cprs
                    .Where(x => x.IdentityId == Guid.Parse(userId))
                    .Select(z => z.PrivateKey)
                    .FirstOrDefaultAsync();

                if (userPrivateKey != null)
                {
                    encryptedTasks = await _dbContext.TodoItems
                        .Where(x => x.IdentityId == Guid.Parse(userId))
                        .ToListAsync();
                }
                //var decryptedTasks = encryptedTasks
                //    .Select(x => new ToDoItem {
                //        Id = x.Id,
                //        IdentityId = x.IdentityId,
                //        Title = _encryptionService
                //            .DecryptAsymmetric(x.Title)
                //    })
                //    .ToList();

                return encryptedTasks;
            }
            catch (Exception ex) { throw new Exception($"Something went wrong while receiving the tasklist {ex}"); }
        }

        public async Task<bool> RemoveItem(string userEmail, Guid itemId)
        {
            string? userId = _context.Users
                .Where(x => x.NormalizedEmail == userEmail.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefault();

            if (userId == null) throw new Exception($"User with email {userEmail} not found");

            ToDoItem item = await _dbContext.TodoItems
                .Where(x => x.Id == itemId && x.IdentityId == Guid.Parse(userId))
                .FirstOrDefaultAsync() ?? throw new Exception($"No item found with id {itemId} for user {userEmail}");

            _dbContext.TodoItems.Remove(item);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AdminDeleteAllTasks(string userEmail)
        {
            string? userId = _context.Users
                .Where(x => x.NormalizedEmail == userEmail.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefault();

            if (userId == null) throw new Exception($"User with email {userEmail} not found");

            List<string> roleIds = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            if (roleIds.Count < 1) throw new Exception($"No roles found for user {userEmail}");

            bool hasAdminRole = await _context.Roles
                .AnyAsync(x => roleIds
                    .Any(r => r == x.Id)
                && x.NormalizedName == "ADMIN");

            if (hasAdminRole)
            {
                List<ToDoItem> userToDoList = await _dbContext.TodoItems
                    .Where(x => x.IdentityId == Guid.Parse(userId))
                    .ToListAsync();

                _dbContext.TodoItems.RemoveRange(userToDoList);
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
