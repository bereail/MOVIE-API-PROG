using MOVIE_API.Models.Enum;

namespace MOVIE_API.Models.DTO
{
    public class MovieAndStateDto
    {
        public int Id { get; set; }

        public int? IdAdmin { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }
        public DateTime? Date { get; set; }

        public MovieState State { get; set; }
    }
}
