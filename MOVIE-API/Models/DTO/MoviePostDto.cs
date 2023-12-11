using MOVIE_API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_api.Models.DTO
{
    public class MoviePostDto
    {
        [Required(ErrorMessage = "El campo 'Title' es requerido.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo 'Director' es requerido.")]
        public string Director { get; set; }

        [Required(ErrorMessage = "El id de 'Admin' es requerido.")]
        public int? IdAdmin { get; set; }

        [JsonIgnore] // Evita que Date se envíe al cliente
        public DateTime? Date { get; set; }

        [JsonIgnore] // Evita que State se envíe al cliente
        public int? State { get; set; } = 1;

        public MoviePostDto()
        {
            // Establecer la fecha actual por defecto si no se proporciona
            Date ??= DateTime.Now;
        }

    }
}

