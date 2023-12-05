using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IngressBkpAutomation.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "User ID")]
        public string? UserID { get; set; }
        public string? Names { get; set; }
        public string? Password { get; set; }
        [NotMapped]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set;}
        public bool Status { get; set; }
        public string? Role { get; set; }
        public string? Personnel { get; set; } 
        public DateTime? DateCreated { get; set; }
    }
}
