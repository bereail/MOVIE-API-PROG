namespace MOVIE_API.Models.DTO
{
    public class BookingResult
    {
        public int BookingId { get; set; }
        public string Message { get; set; }

        public bool Success { get; set; }

        public List<BookingDetailDto> BookingDetails { get; set; }
        public bool UserNotFound { get; set; }
        public bool NoBookings { get; set; }
    }
}
