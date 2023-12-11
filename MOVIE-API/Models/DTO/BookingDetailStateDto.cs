using MOVIE_API.Models.Enum;



//funcion para modificar el estado de una resvera//

namespace MOVIE_API.Models.DTO
{
    public class BookingDetailStateDto
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public BookingDetailState? State { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Comment { get; set; }

        public string NewStateName { get; set; }
    }
}
