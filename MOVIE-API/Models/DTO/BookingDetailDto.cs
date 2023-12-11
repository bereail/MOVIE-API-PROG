using movie_api.Models.DTO;
using MOVIE_API.Models.Enum;

namespace MOVIE_API.Models.DTO
{
    public class BookingDetailDto
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public BookingDetailState? State { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Comment { get; set; }
    }
}
