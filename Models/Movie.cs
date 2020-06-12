using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieZone.Models
{
    public class Movie
    {

        [Key]                                                       // chỉ định khóa chính
        public int Id { get; set; }
        [Required(ErrorMessage = "Movie title is required")]        // trường dữ liệu này không được để trống
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayName("Release Date")]                               // cài đặt tên hiển thị cho thuộc tính
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
    }
}
