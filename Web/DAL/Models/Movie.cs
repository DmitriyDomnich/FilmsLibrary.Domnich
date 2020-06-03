using System;
using System.Collections.Generic;
using Web.ViewModels;

namespace Web.DAL.Models
{
    public class Movie : BaseViewEntity
    {
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Country { get; set; }

        public string Director { get; set; }

        public string Actors { get; set; }

        public string Summary { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public List<ListOrder> ListOrders { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }

        public List<Reservation> Reservations { get; set; }

        public Movie()
        {
            MovieGenres = new List<MovieGenre>();
            ListOrders = new List<ListOrder>();
            Reservations = new List<Reservation>();
        }
    }
}
