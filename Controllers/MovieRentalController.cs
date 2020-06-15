using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieZone.Data;
using MovieZone.Models;

namespace MovieZone.Controllers
{
    public class MovieRentalController : Controller
    {
        private readonly MovieContext _context;

        public MovieRentalController(MovieContext context)
        {
            _context = context;
        }

        // GET: MovieRental
        public async Task<IActionResult> Index()
        {
            var movieContext = _context.MovieRentals.Include(m => m.Movie).Include(m => m.User);
            return View(await movieContext.ToListAsync());
        }

        // GET: MovieRental/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRental = await _context.MovieRentals
                .Include(m => m.Movie)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieRental == null)
            {
                return NotFound();
            }

            return View(movieRental);
        }

        // GET: MovieRental/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: MovieRental/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,MovieId,RentDate,EndDate,Duration,TotalPrice")] MovieRental movieRental)
        {
            // xử lý logic đầu vào
            // Console.WriteLine(movieRental.Duration);
            // Console.WriteLine(TimeSpan.Zero);
            // Console.WriteLine(movieRental.EndDate);

            if (movieRental.Duration != TimeSpan.Zero)                              // Từ Duration tính EndDate
                movieRental.EndDate = movieRental.RentDate + movieRental.Duration;
            else if (movieRental.EndDate != default(DateTime))                      // Từ EndDate tính Duration
                movieRental.Duration = (TimeSpan)(movieRental.EndDate - movieRental.RentDate);
            var tMovie = _context.Movies.Where(m => m.Id == movieRental.MovieId).First();
            movieRental.TotalPrice = tMovie.Price * movieRental.Duration.Days; // Duration.Days : chuyển Duration về dạng int số ngày


            if (ModelState.IsValid && (movieRental.Duration != TimeSpan.Zero || movieRental.EndDate != default(DateTime)))
            {
                _context.Add(movieRental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieRental.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", movieRental.UserId);
            return View(movieRental);
        }

        // GET: MovieRental/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRental = await _context.MovieRentals.FindAsync(id);
            if (movieRental == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieRental.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", movieRental.UserId);
            return View(movieRental);
        }

        // POST: MovieRental/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,MovieId,RentDate,EndDate,Duration,TotalPrice")] MovieRental movieRental)
        {
            if (id != movieRental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieRentalExists(movieRental.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieRental.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", movieRental.UserId);
            return View(movieRental);
        }

        // GET: MovieRental/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRental = await _context.MovieRentals
                .Include(m => m.Movie)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieRental == null)
            {
                return NotFound();
            }

            return View(movieRental);
        }

        // POST: MovieRental/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieRental = await _context.MovieRentals.FindAsync(id);
            _context.MovieRentals.Remove(movieRental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieRentalExists(int id)
        {
            return _context.MovieRentals.Any(e => e.Id == id);
        }
    }
}
