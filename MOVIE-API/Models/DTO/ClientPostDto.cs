using MOVIE_API.Models;
using System.ComponentModel.DataAnnotations;

namespace movie_api.Models.DTO
{
    public class ClientPostDto

    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        // Otras propiedades necesarias para User

        public int BookingId { get; set; }

    }
}

