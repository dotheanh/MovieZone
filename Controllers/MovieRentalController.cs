using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieZone.Data;
using MovieZone.Models;
using MovieZone.ViewModels;
using Microsoft.AspNetCore.Http;

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

        ////////////////////////// trang biểu đồ cho thuê phim
        public async Task<IActionResult> Charts()
        {
            var movieContext = _context.MovieRentals;
            return View(await movieContext.ToListAsync());
        }

        /////////////// Movie Rental gọi ajax
        public IActionResult mrAJAX()
        {
            return View();
        }

        public List<MovieRentalViewModel> getData()
        {
            var movieContext = _context.MovieRentals.Include(m => m.Movie).Include(m => m.User);
            List<MovieRentalViewModel> viewModel = new List<MovieRentalViewModel>();        // tạo ActionResult theo kiểu ViewModel, lấy dòng dữ liệu đầu từ context

            foreach (var mContext in movieContext)
            {
                var temp = new MovieRentalViewModel
                {
                    movie = mContext.Movie,
                    user = mContext.User,
                    movieRental = new MovieRental
                    {
                        RentDate = mContext.RentDate,
                        EndDate = mContext.EndDate,
                        Duration = mContext.Duration,
                        TotalPrice = mContext.TotalPrice
                    }
                };

                viewModel.Add(temp);
            }

            return viewModel; // viewModel là List<MovieRentalViewModel>
        }


        /////////////// trang Movie Rental dùng ViewModel
        public async Task<IActionResult> mrvm()
        {
            var movieContext = await _context.MovieRentals.Include(m => m.Movie).Include(m => m.User).ToListAsync();
            List<MovieRentalViewModel> viewModel = new List<MovieRentalViewModel>();        // tạo ActionResult theo kiểu ViewModel, lấy dòng dữ liệu đầu từ context

            foreach (var mContext in movieContext)
            {
                var temp = new MovieRentalViewModel
                {
                    movie = mContext.Movie,
                    user = mContext.User,
                    movieRental = new MovieRental
                    {
                        RentDate = mContext.RentDate,
                        EndDate = mContext.EndDate,
                        Duration = mContext.Duration,
                        TotalPrice = mContext.TotalPrice
                    }
                };

                viewModel.Add(temp);
            }

            return View(viewModel); // viewModel là List<MovieRentalViewModel>
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
            if (HttpContext.Session.GetString("idUser") != null)
            {
                return View();
            }
            else
            {
                return PartialView("~/Views/User/Login.cshtml");
            }
        }

        // POST: MovieRental/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,MovieId,RentDate,EndDate,Duration,TotalPrice")] MovieRental movieRental)
        {
            // xử lý logic đầu vào


            if (movieRental.Duration != TimeSpan.Zero)                              // Từ Duration tính EndDate
                movieRental.EndDate = movieRental.RentDate + movieRental.Duration;
            else if (movieRental.EndDate != default(DateTime) && movieRental.EndDate != null)                      // Từ EndDate tính Duration
                movieRental.Duration = (TimeSpan)(movieRental.EndDate - movieRental.RentDate);
            var tMovie = _context.Movies.Where(m => m.Id == movieRental.MovieId).First();
            movieRental.TotalPrice = tMovie.Price * movieRental.Duration.Days; // Duration.Days : chuyển Duration về dạng int số ngày


            if (ModelState.IsValid && (movieRental.Duration != TimeSpan.Zero || (movieRental.EndDate != default(DateTime) && movieRental.EndDate != null)))
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
