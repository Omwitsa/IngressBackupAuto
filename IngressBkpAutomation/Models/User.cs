using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IngressBkpAutomation.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        [StringLength(50)]
        public string? UserID { get; set; }
        [StringLength(50)]
        public string? Names { get; set; }
        [StringLength(100)]
        public string? Password { get; set; }
        [NotMapped]
        [Display(Name = "Confirm Password")]
        [StringLength(100)]
        public string? ConfirmPassword { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(20)]
        public string? Phone { get; set;}
        public bool Status { get; set; }
        [StringLength(20)]
        public string? Role { get; set; }
        [StringLength(20)]
        public string? Personnel { get; set; } 
    }
}
