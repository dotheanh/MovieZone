using MovieZone.Models;

namespace MovieZone.ViewModels
{
    public class MovieRentalViewModel
    {
        public Movie movie { get; set; }
        public User user { get; set; }
        public MovieRental movieRental { get; set; }

        // public List<Movie> Movies { get; set; }
        // public List<User> Users { get; set; }
        // public List<MovieRental> MovieRentals { get; set; }
    }
}

