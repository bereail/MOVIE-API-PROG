using movie_api.Models.DTO;

namespace MOVIE_API.Models.DTO
{
    public class ClassifiedMoviesDto
    {
        public List<MovieDto> AvailableMovies { get; set; }
        public List<MovieDto> DeletedOrSuspendedMovies { get; set; }
    }
}
