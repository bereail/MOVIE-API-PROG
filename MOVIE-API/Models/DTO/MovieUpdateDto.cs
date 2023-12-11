namespace MOVIE_API.Models.DTO
{
    public class MovieUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Director { get; set; }
        public DateTime? Date { get; set; }

        public int? State { get; set; }
    }
}
