using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class MovieCardViewModel : BaseViewEntity
    {
        public string Name { get; set; }

        public string Director { get; set; }

        public DateTime RealeseDate { get; set; }

        public int Price { get; set; }

        public string Image { get; set; }

        public List<string> MovieGenres { get; set; }

        public MovieCardViewModel()
        {
            MovieGenres = new List<string>();
        }
    }
}
