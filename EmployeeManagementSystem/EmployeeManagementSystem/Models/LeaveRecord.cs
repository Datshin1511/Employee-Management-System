using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public record LeaveRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; init; }

        [Column("EmployeeID")]
        [MaxLength(10)]
        public string EmployeeID { get; init; }

        [Column("LeaveReason")]
        [MaxLength(400)]
        public string LeaveReason { get; init; }

        [Column("LeaveStatusMsg")]
        [MaxLength(10)]
        public string LeaveStatusMsg { get; init; }

        [Column("LeaveStatus")]
        public bool LeaveStatus { get; init; }

        [Column("TicketDate")]
        public DateTime TicketDate { get; init; }
    }
}
