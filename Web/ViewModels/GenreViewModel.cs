using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DAL.Models;
using Web.Models.DAL;

namespace Web.ViewModels
{
    public class GenreViewModel : BaseViewEntity
    {
        public int Name { get; set; }
        public List<int> MovieGenres { get; set; }
        public GenreViewModel()
        {
            MovieGenres = new List<int>();
        }
    }
}
