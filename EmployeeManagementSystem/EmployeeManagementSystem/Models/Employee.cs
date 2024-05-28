using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    [Table("Employee")]
    public record Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; init; }

        [Column("EmployeeID")]
        [MaxLength(10)]
        public string EmployeeID{ get; init; }

        [Column("EmployeeType")]
        [MaxLength(10)]
        public string EmployeeType { get; init; }

        [Column("FirstName")]
        [MaxLength(20)]
        public string FirstName { get; init; }

        [Column("LastName")]
        [MaxLength(20)]
        public string LastName { get; init; }

        [Column("Age")]
        public int Age { get; init; }

        [Column("PhoneNumber")]
        [MaxLength(13)]
        public string PhoneNumber { get; init; }

        [Column("EmailID")]
        [MaxLength(30)]
        public string EmailID { get; init; }
        
    }
}
