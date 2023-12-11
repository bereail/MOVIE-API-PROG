using movie_api.Models.DTO;

namespace MOVIE_API.Models.DTO
{
    public class BookingDetailPostDto
    {

        public int? IdMovie { get; set; }

        public int? State = 1;

        public DateTime? BookingDate = DateTime.Now;

        public DateTime? ReturnDate = DateTime.Now.AddHours(48);

        public string Comment { get; set; }
    }
}


