using System.Collections.Generic;
using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }
        public Genre()
        {
            MovieGenres = new List<MovieGenre>();
        }
    }
}