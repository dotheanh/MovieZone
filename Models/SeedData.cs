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
                ////////////////////////////////////////////////////////// Seed data cho Categories
                if (context.Categories.Any()) { return; }
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Comedy"
                    },
                    new Category
                    {
                        Name = "Heroes"
                    },
                    new Category
                    {
                        Name = "Action"
                    },
                    new Category
                    {
                        Name = "Drama"
                    }
                );
                context.SaveChanges();

                ////////////////////////////////////////////////////////// Seed data cho Movies?????????????????????????
                // if (context.Movies.Any()) { return; }
                // context.Movies.AddRange(
                //     new Movie
                //     {
                //         Title = "Avengers",
                //         Description = "Incredible Heroes",
                //         ReleaseDate = DateTime.Parse("1989-2-12"),
                //         Price = 3,
                //         CategoryId = 0,
                //         Category = context.Categories.Find(0)
                //     }
                // );
                // context.SaveChanges();
            }
        }
    }
}