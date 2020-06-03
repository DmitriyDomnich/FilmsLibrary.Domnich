using Microsoft.EntityFrameworkCore;
using Web.DAL.Models;

namespace Web.DAL
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ListOrder> ListOrders { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Reservation> Reservations { get; set; }


        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(t => new { t.MovieId, t.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.Genre)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(m => m.GenreId);

        }
    }
}
