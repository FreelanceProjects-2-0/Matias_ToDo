using System.ComponentModel.DataAnnotations;

namespace Matias_ToDo_DoubleDB.Data.Models.Entities
{
    public class ToDoItem
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
