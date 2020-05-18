using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DAL.Models;

namespace Web.DAL
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {

        }
    }
}
