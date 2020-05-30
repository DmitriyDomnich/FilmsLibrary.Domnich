using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class MovieListViewModel : BaseViewEntity
    {
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Country { get; set; }

        public string Director { get; set; }

        public int Price { get; set; }

        public List<string> MovieGenres { get; set; }

        public MovieListViewModel()
        {

            MovieGenres = new List<string>();
        }
    }
}