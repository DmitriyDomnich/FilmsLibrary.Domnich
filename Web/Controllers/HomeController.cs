using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, LibraryDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            db = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 9;

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

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = result
            };

            var userName = this.HttpContext.User.Identity.Name;
            ViewBag.ReservationsCount = await this.db.Reservations.Where(f => f.User.Email == userName).SumAsync(f => f.Amount);

            return View(viewModel);
        }
        //[Authorize]
        //public async Task<IActionResult> GenresView()
        //{
        //    var 
        //    return View();
        //}
        [Authorize]
        public async Task<IActionResult> MovieSearch()
        {
            string movieName = Request.Form.FirstOrDefault(f => f.Key == "name").Value;
            var movies = await this.db.Movies.Where(f => f.Name.Contains(movieName)).ToListAsync();

            if (movies.Count == 0)
            {
                ViewBag.Message = "Фильма с таким названием не найдено.";
                return View("~/Views/Home/NullEntityView.cshtml");
            }

            var result = movies.Select(f => new MovieCardViewModel
            {
                MovieGenres = f.MovieGenres.Select(f => f.Genre.Name).ToList(),
                Name = f.Name,
                Director = f.Director,
                Id = f.Id,
                Image = f.Image,
                Price = f.Price,
                ReleaseDate = f.ReleaseDate
            }).ToList();

            


            ViewBag.Name = movieName;
            return View(result);
        }

        [Authorize]
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
        [Authorize]
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

            if (result.Count == 0)
            {
                ViewBag.Message = "Фильмы такого жанра отсутствуют, наверное они редкие.";
                return View("~/Views/Home/NullEntityView.cshtml");
            }

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = result
            };

            return View(viewModel);
        }
        [Authorize]
        public async Task<IActionResult> CartView()
        {
            var userName = this.HttpContext.User.Identity.Name;
            var user = await this.db.Users
                .Include(f => f.Reservations)
                .ThenInclude(f => f.Movie)
                .FirstOrDefaultAsync(f => f.Email == userName);
   

            List<ShoppingCartViewModel> result = user.Reservations.Select(f => new ShoppingCartViewModel
            {
                MovieId = f.MovieId,
                Name = f.Movie.Name,
                Image = f.Movie.Image,
                Price = f.Movie.Price,
                Amount = f.Amount
            }).ToList();

            if (result.Count == 0)
            {
                ViewBag.Message = "Ваша корзина пуста.";
                return View("~/Views/Home/NullEntityView.cshtml");
            }

            return View(result);
        }

        public async Task<IActionResult> CreateListOrders()
        {
            var user = await this.GetUserAsync();

            Order order = new Order()
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                ListOrders = user.Reservations.Select(f => new ListOrder
                {
                    Movie = f.Movie,
                    MovieId = f.Id,
                    Amount = f.Amount,
                }).ToList()
            };
            var reservations = await db.Reservations.Where(f => f.UserId == user.Id).ToListAsync();
            db.Reservations.RemoveRange(reservations);
            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return View();
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

        private async Task<User> GetUserAsync()
        {
            var userName = this.HttpContext.User.Identity.Name;
            var user = await this.db.Users
                .Include(f => f.Reservations)
                .ThenInclude(f => f.Movie)
                .FirstOrDefaultAsync(f => f.Email == userName);

            return user;
        }
    }
}
