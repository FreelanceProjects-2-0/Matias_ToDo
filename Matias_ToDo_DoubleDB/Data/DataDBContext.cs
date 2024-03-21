using Matias_ToDo_DoubleDB.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Matias_ToDo_DoubleDB.Data;

public class DataDBContext(DbContextOptions<DataDBContext> options) : DbContext(options)
{
    public DbSet<Cpr> Cprs { get; set; }
    public DbSet<ToDoItem> TodoItems { get; set; }
}
