using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieZone.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")] 
        public string Password { get; set; }
 
        [NotMapped] // thuộc tính này ko được ánh xạ vào DB
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
