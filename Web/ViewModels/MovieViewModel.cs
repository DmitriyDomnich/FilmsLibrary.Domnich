using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models.DAL;

namespace Web.ViewModels
{
    public class MovieViewModel : BaseViewEntity
    {
        [Required(ErrorMessage = "Не указано название фильма.")]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Country { get; set; }

        public string Director { get; set; }

        public string Actors { get; set; }
        
        public string Summary { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public List<int> MovieGenres { get; set; }

        public MovieViewModel()
        {
            MovieGenres = new List<int>();
        }
    }
}
