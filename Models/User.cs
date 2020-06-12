using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    }
}
