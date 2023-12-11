using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using System.Collections.Generic;

namespace movie_api.Services.Interfaces
{
    public interface IBookingService
    {




        //// Verifica si el usuario tiene reservas o ninguna y las clasifica en actuales e historial
        List<BookingDetailDto> GetBookingsAndDetailsByUserId(int userId);




        //crea un nueva booking detail ingrenado el id de un usuario
        BookingResult CreateBookingDetail(int userId, BookingDetailPostDto bookingDetailDto);




        //trae todas las reservas y las ordena segun su state
        List<BookingDetailDto> GetBookingDetails();

        //trae todas las reservas actuales de un usuairo ingrensado su id
        List<BookingDetailDto> GetCurrentReservedBookingsByUserId(int userId);


        //editar el estado de una reserva
        bool UpdateBookingDetailState(int bookingDetailId, BookingDetailState newState);

        /*List<ClientDto> GetAllClientsWithBookings();*/
        //
        /* BookingResult CreateBookingDetail(int userId, BookingDetailDto bookingDetailDto);*/
    }
}





//verifica si el usuario existe, no existe o no tiene reservas asociadas y devuelve su booking id
/*BookingResult CheckBookingByIdUser(int userId);*/
