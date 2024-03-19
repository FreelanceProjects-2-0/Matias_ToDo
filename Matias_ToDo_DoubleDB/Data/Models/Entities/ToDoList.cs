using System.ComponentModel.DataAnnotations;

namespace Matias_ToDo_DoubleDB.Data.Models.Entities
{
    public class ToDoList
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdentityId { get; set; }

        public required List<ToDoItem> Items { get; set; }
    }
}
