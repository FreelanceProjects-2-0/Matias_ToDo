using System.ComponentModel.DataAnnotations;

namespace Matias_ToDo_DoubleDB.Data.Models.Entities
{
    public class Cpr
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdentityId {  get; set; }

        public required string CprNumber { get; set; }

        public string UserMail { get; set; } = string.Empty;

        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
    }
}
