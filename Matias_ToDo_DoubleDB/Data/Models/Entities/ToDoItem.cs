using System.ComponentModel.DataAnnotations;

namespace Matias_ToDo_DoubleDB.Data.Models.Entities
{
    public class ToDoItem
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public Guid IdentityId { get; set; }
    }
}
