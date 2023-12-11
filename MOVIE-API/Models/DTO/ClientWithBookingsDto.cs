using movie_api.Models.DTO;

namespace MOVIE_API.Models.DTO
{
    public class ClientWithBookingsDto : UserDto
    {
        public int Id { get; set; }

        public List<int> BookingIds { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

        public int? Rol { get; set; }
    }
}
