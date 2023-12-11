namespace MOVIE_API.Models.DTO
{
    public class UserCreateDto
    {

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

        public int? Rol { get; set; }
    }
}
