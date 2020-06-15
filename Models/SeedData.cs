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
                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(
                        new Movie
                        {
                            Title = "AVENGERS: ENDGAME",
                            Description = "The grave course of events set in motion by Thanos",
                            ReleaseDate = DateTime.Parse("2018-2-12"),
                            Price = 2.7M,
                            Category = new Category("Heroes")
                        },
                        new Movie
                        {
                            Title = "Parasite",
                            Description = "Bong Joon Ho brings his work home to Korea",
                            ReleaseDate = DateTime.Parse("2019-11-2"),
                            Price = 2.2M,
                            Category = new Category("Drama")
                        },
                        new Movie
                        {
                            Title = "Mr. Bean",
                            Description = "Hahahaahaa",
                            ReleaseDate = DateTime.Parse("2003-5-26"),
                            Price = 1M,
                            Category = new Category("Comedy")
                        },
                        new Movie
                        {
                            Title = "Fast & Furious",
                            Description = "Fast & Furious is a series of action films",
                            ReleaseDate = DateTime.Parse("2020-5-1"),
                            Price = 4.2M,
                            Category = new Category("Action")
                        }
                    );
                    context.SaveChanges();

                    context.Movies.AddRange(
                        new Movie
                        {
                            Title = "AVENGERS: Age of Ultron",
                            Description = "The grave course of events set in motion by Thanos",
                            ReleaseDate = DateTime.Parse("2014-11-20"),
                            Price = 2.7M,
                            Category = context.Categories.Where(c => c.Name == "Heroes").First()
                        },
                        new Movie
                        {
                            Title = "Spider-Man",
                            Description = "Spider-Man is a fictional superhero",
                            ReleaseDate = DateTime.Parse("2017-1-5"),
                            Price = 1.8M,
                            Category = context.Categories.Where(c => c.Name == "Action").First()
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            UserName = "nhoxtheanh",
                            FullName = "Đỗ Thế Anh"
                        },
                        new User
                        {
                            UserName = "nhoxtheem",
                            FullName = "Đỗ Thế Em"
                        }
                    );
                    context.SaveChanges();
                }

                //if (!context.MovieRentals.Any())//// ???????????//
                {
                    context.MovieRentals.AddRange(
                        new MovieRental
                        {
                            User = context.Users.Where(u => u.FullName == "Đỗ Thế Anh").First(),
                            Movie = context.Movies.Where(m => m.Title == "AVENGERS: Age of Ultron").First(),
                            RentDate = DateTime.Parse("2020-6-15"),
                            EndDate = DateTime.Parse("2020-6-13"),
                            Duration = TimeSpan.FromDays(3),
                            TotalPrice = 6.7M
                        },
                        new MovieRental
                        {
                            User = context.Users.Where(u => u.FullName == "Đỗ Thế Em").First(),
                            Movie = context.Movies.Where(m => m.Title == "Mr. Bean").First(),
                            RentDate = DateTime.Parse("2020-6-15"),
                            EndDate = DateTime.Parse("2020-6-20"),
                            Duration = TimeSpan.FromDays(5),
                            TotalPrice = 5M
                        }
                    );
                    context.SaveChanges();
                }

            }
        }
    }
}