namespace MOVIE_API.Models.DTO
{
    public class BookingPostDto
    {

        public int IdBooking { get; set; }
        
        public int? IdUser { get; set; }

        public DateTime? Date { get; set; }
    }
}
