namespace MOVIE_API.Models.DTO
{
    public class BookingDetailCreateDto
    {
        public int IdMovie { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Comments { get; set; }
    }
}
