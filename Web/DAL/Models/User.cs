using System.Collections.Generic;
using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Phone { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public List<Order> Orders { get; set; }

        public List<Reservation> Reservations { get; set; }

        public User()
        {
            Orders = new List<Order>();
            Reservations = new List<Reservation>();
        }
    }
}
