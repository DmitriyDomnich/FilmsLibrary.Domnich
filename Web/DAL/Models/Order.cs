using System;
using System.Collections.Generic;
using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ListOrder> ListOrders { get; set; }
        public Order()
        {
            ListOrders = new List<ListOrder>();
        }
    }
}