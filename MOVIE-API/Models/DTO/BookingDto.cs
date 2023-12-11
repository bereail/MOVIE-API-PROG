using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MOVIE_API.Models.DTO;

namespace movie_api.Models.DTO
{
    public class BookingDto
    {

        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdBookingDetail { get; set; }
        public DateTime? Date { get; set; }
    }
}

