using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.DAL;
using Web.DAL.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryDbContext db;

        public HomeController(ILogger<HomeController> logger, LibraryDbContext context)
        {
            _logger = logger;
            db = context;
        }
        
        public async Task<IActionResult> Index(int page = 1)
        {
            //return RedirectToAction("Index", "Administrator");

            int pageSize = 9;
            //string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var count = await this.db.Movies.CountAsync();

            var movies = this.db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre);
            var items = await movies.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            List<MovieCardViewModel> result = items.Select(f => new MovieCardViewModel
            {
                Id = f.Id,
                Director = f.Director,
                Image = f.Image,
                Name = f.Name,
                ReleaseDate = f.ReleaseDate,
                Price = f.Price,
                MovieGenres = f.MovieGenres.Select(f => f.Genre.Name).ToList()
            }).ToList();

            ViewBag.Genres = db.Genres.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = result
            };

            return View(viewModel);
        }
        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = await db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre).FirstOrDefaultAsync(p => p.Id == id);

            if (movie != null)
            {
                var result = new MovieDetailsViewModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    RealeaseDate = movie.ReleaseDate,
                    MovieGenres = movie.MovieGenres.Select(f => f.Genre.Name).ToList(),
                    Actors = movie.Actors,
                    Country = movie.Country,
                    Director = movie.Director,
                    Image = movie.Image,
                    Price = movie.Price,
                    Summary = movie.Summary
                };
                ViewData["Title"] = result.Name;
                return View(result);
            }

            return NotFound();
        }
        public async Task<IActionResult> MoviesOfGenre(int id, int page = 1)
        {
            int pageSize = 9;

            var movies = await db.Movies
                .Where(f => f.MovieGenres.Any(g => g.GenreId == id)).Include(f => f.MovieGenres).ThenInclude(f => f.Genre).ToListAsync();

            var count = movies.Count();
            var items = movies.Skip((page - 1) * pageSize).Take(pageSize);


            List<MovieCardViewModel> result = items.Select(f => new MovieCardViewModel
            {
                MovieGenres = f.MovieGenres.Select(f => f.Genre.Name).ToList(),
                Name = f.Name,
                Director = f.Director,
                Id = f.Id,
                Image = f.Image,
                Price = f.Price,
                ReleaseDate = f.ReleaseDate
            }).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = result
            };

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
