using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.DAL;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public ShoppingCartController(LibraryDbContext context)
        {
            this._db = context;
        }

        [HttpPost("{movieId}")]
        public async Task<IActionResult> CreateOrder(int movieId)
        {
            var userName = this.HttpContext.User.Identity.Name;
            var user = await this._db.Users
                .Include(f => f.Reservations)
                .FirstOrDefaultAsync(f => f.Email == userName);

            var reservation = user.Reservations.FirstOrDefault(f => f.MovieId == movieId);

            if (reservation != null)
            {
                reservation.Amount++;
            }
            else
            {
                user.Reservations.Add(new DAL.Models.Reservation { Amount = 1, MovieId = movieId, UserId = user.Id });
            }

            await this._db.SaveChangesAsync();

            return this.Ok();
        }
        [HttpDelete("{movieId}")]
        public async Task<IActionResult> DeleteOrder(int movieId)
        {          
            var userName = this.HttpContext.User.Identity.Name;
            var user = await this._db.Users
                .Include(f => f.Reservations)
                .FirstOrDefaultAsync(f => f.Email == userName);

            var reservation = user.Reservations.FirstOrDefault(f => f.MovieId == movieId);

            if (reservation != null)
            {
                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}