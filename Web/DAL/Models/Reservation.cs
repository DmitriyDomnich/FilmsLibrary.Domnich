using Web.Models.DAL;

namespace Web.DAL.Models
{
    public class Reservation : BaseEntity
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int Amount { get; set; }
    }
}
