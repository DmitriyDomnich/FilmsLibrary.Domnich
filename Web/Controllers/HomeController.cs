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

            int pageSize = 6;

            var count = await this.db.Movies.CountAsync();
            
            var movies = this.db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre);
            var items = await movies.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            List<MovieCardViewModel> result = items.Select(f => new MovieCardViewModel
            {
                Id = f.Id,
                Director = f.Director,
                Image = f.Image,
                Name = f.Name,
                RealeseDate = f.ReleaseDate,
                Price = f.Price,
                MovieGenres = f.MovieGenres.Select(f => f.Genre.Name).ToList()
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
