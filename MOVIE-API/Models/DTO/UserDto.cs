

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MOVIE_API.Models.Enum;

namespace movie_api.Models.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

        public UserRole? Rol { get; set; }
    }
}

