using Microsoft.EntityFrameworkCore;
using MovieZone.Models;

namespace MovieZone.Data{
    public class MovieContext : DbContext{
        public MovieContext(DbContextOptions<MovieContext> options) : base(options){    // chưa hiểu

        }

        public DbSet<Movie> Movies {get; set;}  // Tập các thực thể <Movie> đại diện cho DB set lấy từ DB
    }
}