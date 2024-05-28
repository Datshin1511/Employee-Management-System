using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public record Credential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; init; }

        [Column("EmailID")]
        [MaxLength(30)]
        public string EmailID { get; init; }

        [Column("EmailPassword")]
        [MaxLength(30)]
        [PasswordPropertyText]
        public string EmailPassword { get; init; }

    }
}
