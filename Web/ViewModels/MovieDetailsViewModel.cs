using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class MovieDetailsViewModel : BaseViewEntity
    {
        public string Name { get; set; }

        public string Director { get; set; }

        public string Country { get; set; }

        public DateTime RealeaseDate { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public List<string> MovieGenres { get; set; }

        public string Actors { get; set; }

        public string Summary { get; set; }

        public int Amount { get; set; }

        public MovieDetailsViewModel()
        {
            MovieGenres = new List<string>();
        }
    }
}
