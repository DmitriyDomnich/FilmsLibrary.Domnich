using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.DAL;
using Web.DAL.Models;
using Web.Utils;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly LibraryDbContext db;

        private readonly IHostingEnvironment environment;

        public AdministratorController(LibraryDbContext context, IHostingEnvironment environment)
        {
            this.db = context;
            this.environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await this.db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre).ToListAsync();
            List<MovieListViewModel> result = movies.Select(f => new MovieListViewModel
            {
                Id = f.Id,
                Country = f.Country,
                Director = f.Director,
                Name = f.Name,
                Price = f.Price,
                ReleaseDate = f.ReleaseDate,
                MovieGenres = f.MovieGenres.Select(f => f.Genre.Name).ToList(),
            }).ToList();
            return View(result);
        }
        public IActionResult AddMovie()
        {
            ViewBag.Genres = db.Genres.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(MovieViewModel model)
        {
            var newMovie = new Movie
            {
                Name = model.Name,
                Country = model.Country,
                MovieGenres = model.MovieGenres.Select(d => new MovieGenre { GenreId = d }).ToList(),
                Actors = model.Actors,
                Price = model.Price,
                Summary = model.Summary,
                Director = model.Director,
                ReleaseDate = model.ReleaseDate,
            };

            var file = this.HttpContext.Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var fileName = ImageHelper.UploadImage(file, Path.Combine(this.environment.WebRootPath, "MovieImages"));
                newMovie.Image = fileName;
            }

            db.Movies.Add(newMovie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateMovieView(int id)
        {
            var movie = await db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre).FirstOrDefaultAsync(p => p.Id == id);

            if (movie != null)
            {
                var result = new MovieViewModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    ReleaseDate = movie.ReleaseDate,
                    MovieGenres = movie.MovieGenres.Select(f => f.GenreId).ToList(),
                    Actors = movie.Actors,
                    Country = movie.Country,
                    Director = movie.Director,
                    Image = movie.Image,
                    Price = movie.Price,
                    Summary = movie.Summary
                };

                ViewBag.Genres = db.Genres.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = result.MovieGenres.Contains(r.Id)
                }).ToList();

                return View(result);
            }

            return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> UpdateMovie(MovieViewModel model) //TODO: Убрать Id с модели и добавить как параметр енд поинта и поменять на PUT
        {
            var movie = await db.Movies.Include(f => f.MovieGenres).ThenInclude(f => f.Genre).FirstOrDefaultAsync(p => p.Id == model.Id);
            if (movie != null)
            {
                movie.MovieGenres = movie.MovieGenres.Where(f => model.MovieGenres.Contains(f.GenreId)).ToList();
                var newItems = model.MovieGenres.Except(movie.MovieGenres.Select(f => f.GenreId)).Select(gId => new MovieGenre
                {
                    GenreId = gId
                }).ToList();

                movie.MovieGenres.AddRange(newItems);
                movie.Name = model.Name;
                movie.Price = model.Price;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Summary = model.Summary;
                movie.Director = model.Director;

                var file = this.HttpContext.Request.Form.Files.FirstOrDefault();
                if (file != null && file.Length > 0)
                {
                    var newFileName = ImageHelper.UploadImage(file, Path.Combine(this.environment.WebRootPath, "MovieImages"));
                    movie.Image = newFileName;
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Administrator");
            }

            return NotFound();
        }
        public async Task<IActionResult> DeleteMovieView(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(f => f.Id == id);
            if (movie != null)
            {
                var result = new MovieDeleteViewModel
                {
                    Id = movie.Id,
                    Name = movie.Name
                };
                return View(result);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(f => f.Id == id);

            if (movie != null)
            {
                db.Movies.Remove(movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Administrator");
            }
            return NotFound();
        }
    }
}