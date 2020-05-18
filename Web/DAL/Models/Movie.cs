using System;
using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class Movie : BaseEntity
    {
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Country { get; set; }

        public string Director { get; set; }
    }
}
