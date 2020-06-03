using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DAL.Models;

namespace Web.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string Name { get; set; }

        public int MovieId { get; set; }

        public string Image { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        
    }
}
