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
        [Required(ErrorMessage = "Не указана дата.")]

        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Не указана страна.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Не указан режиссёр.")]
        public string Director { get; set; }
        [Required(ErrorMessage = "Не указаны актеры.")]
        public string Actors { get; set; }
        
        public string Summary { get; set; }
        [Required(ErrorMessage = "Не указана цена.")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Не выбрано изображение.")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Не указаны жанры.")]
        public List<int> MovieGenres { get; set; }

        public MovieViewModel()
        {
            MovieGenres = new List<int>();
        }
    }
}
