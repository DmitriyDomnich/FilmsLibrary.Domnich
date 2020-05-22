using System.Collections.Generic;
using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class ListOrder : BaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Amount { get; set; }
        
    }
}