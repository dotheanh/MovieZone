using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MovieZone.Data;
using Microsoft.EntityFrameworkCore;

namespace MovieZone.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieContext(serviceProvider.GetRequiredService<DbContextOptions<MovieContext>>()))
            {
                if (context.Movies.Any()) { return; }
                context.Movies.AddRange(
                    new Movie
                    {
                        Title = "Avengers",
                        Description = "Incredible Heroes",
                        ReleaseDate = new DateTime(2008, 5, 1),
                        Price = 3,
                        Category = new Category("Heros")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}