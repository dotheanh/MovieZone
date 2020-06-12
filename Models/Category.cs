using System.ComponentModel.DataAnnotations;

namespace MovieZone.Models
{

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        public Category(string Name){
            this.Name = Name;
        }
    }
}
