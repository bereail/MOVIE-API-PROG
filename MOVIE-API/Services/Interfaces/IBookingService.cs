using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using System.Collections.Generic;

namespace movie_api.Services.Interfaces
{
    public interface IBookingService
    {

        //// trae todas las reservas asociadas a un userId y la ordena segun su estado actuales o historial
        List<BookingDetailDto> GetBookingsAndDetailsByUserId(int userId);


        //crea un nueva booking detail ingrenado el id de un usuario
        BookingResult CreateBookingDetail(int userId, BookingDetailPostDto bookingDetailDto);



        //trae todas las reservas actuales de un usuairo ingrensado su id
        public List<BookingDetailDto> GetBookingsAndDetailsByUserIdFromFrontend(int userId);


        //editar el estado de una reserva
        bool UpdateBookingDetailState(int bookingDetailId, BookingDetailState newState);

   
    }
}

/*        //trae todas las reservas y las ordena segun su state
        List<BookingDetailDto> GetBookingDetails();*/