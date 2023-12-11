using Microsoft.EntityFrameworkCore;
using movie_api.Models;
using movie_api.Models.DTO;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace movie_api.Services
{
    public class BookingService : IBookingService
    {
        private readonly moviedbContext _movieDbContext;

        public BookingService(moviedbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }




        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //// Verifica si el usuario tiene reservas o ninguna y las clasifica en actuales e historial
        public List<BookingDetailDto> GetBookingsAndDetailsByUserId(int userId)
        {
            var userExists = _movieDbContext.Users.Any(u => u.Id == userId);

            if (!userExists)
            {
                Console.WriteLine($"Advertencia: Usuario con ID {userId} no encontrado.");
                return new List<BookingDetailDto>();
            }

            var currentDate = DateTime.Now;

            var bookingsAndDetails = _movieDbContext.BookingDetails
                .Include(bd => bd.IdBookingNavigation)
                .Include(bd => bd.IdMovieNavigation)
                .Where(bd => bd.IdBookingNavigation.IdUser == userId)
                .Select(bd => new BookingDetailDto
                {
                    MovieTitle = bd.IdMovieNavigation.Title,
                    State = bd.State,
                    BookingDate = bd.BookingDate,
                    ReturnDate = bd.ReturnDate,
                    Comment = bd.Comment,
                })
                .ToList();

            var currentBookings = bookingsAndDetails
     .Where(bd => bd.ReturnDate == null || (bd.ReturnDate > currentDate && bd.State == BookingDetailState.Reserved))
     .ToList();

            var historicalBookings = bookingsAndDetails
                .Where(bd => bd.ReturnDate != null && (bd.ReturnDate <= currentDate || bd.State != BookingDetailState.Reserved))
                .ToList();

            return currentBookings.Concat(historicalBookings).ToList();
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //crea un nueva booking detail asociada a un id user -> el usuario se logea y genera la reserva luego le lleva le peli o la busca
        public BookingResult CreateBookingDetail(int userId, BookingDetailPostDto bookingDetailDto)
        {
            try
            {
                // Verificar si el usuario existe
                var userExists = _movieDbContext.Users.Any(u => u.Id == userId);

                if (!userExists)
                {
                    // Manejar el caso en el que el usuario no existe
                    return new BookingResult
                    {
                        Message = "El usuario con el ID especificado no existe.",
                    };
                }

                // Buscar la reserva asociada al usuario
                var booking = _movieDbContext.Bookings.FirstOrDefault(b => b.IdUser == userId);

                if (booking == null)
                {
                    // Si el usuario no tiene una reserva, crea una nueva reserva
                    booking = new Booking
                    {
                        IdUser = userId,
                        Date = DateTime.Now,
                    };

                    _movieDbContext.Bookings.Add(booking);
                    _movieDbContext.SaveChanges();
                }

                // Aquí puedes mapear bookingDetailDto a tu entidad BookingDetail y asignar el IdBooking
                var bookingDetailEntity = new BookingDetail
                {
                    IdMovie = bookingDetailDto.IdMovie,
                    Comment = bookingDetailDto.Comment,
                    IdBooking = booking.Id, // Asigna el ID de la reserva asociada al usuario


                    // Asigna los valores State, date, y return date predeterminados
                    // Asigna los valores predeterminados
                    BookingDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddHours(48),
                    State = BookingDetailState.Reserved

                };

                _movieDbContext.BookingDetails.Add(bookingDetailEntity);
                _movieDbContext.SaveChanges();


                // Formatea la fecha de retorno en un formato legible
                var formattedReturnDate = bookingDetailEntity.ReturnDate?.ToString("MM-dd HH:mm");

                // Verifica si la fecha de retorno se formateó correctamente antes de incluirla en el mensaje
                var responseMessage = $"Reserva creada exitosamente. La película debe ser devuelta antes del {(formattedReturnDate ?? "No disponible")}.";

                return new BookingResult
                {
                    Success = true,
                    Message = responseMessage,
                    BookingId = bookingDetailEntity.Id
                };
            }
            catch (InvalidOperationException ex)
            {
                // Captura excepciones específicas
                throw new InvalidOperationException("Error al crear el BookingDetail.", ex);
            }
        }



        // -----------------------------------------------------------------------------------------------------

        //trae todas las reservas y sus detalles 
        public List<BookingDetailDto> GetBookingDetails()
        {
          try
            {
                var bookingDetailDtos = _movieDbContext.BookingDetails
                    .Select(MapToDto)
                    .ToList();

                return bookingDetailDtos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al obtener películas: {ex.Message}");
                return new List<BookingDetailDto>();
            }
        }

        private BookingDetailDto MapToDto(BookingDetail bookingDetail)
        {
            return new BookingDetailDto
            {
                BookingDate = bookingDetail.BookingDate,
                ReturnDate = bookingDetail.ReturnDate,
                Comment = bookingDetail.Comment,
                State = bookingDetail.State // Explicit conversion here
            };
        }




        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------------------------------------------------------------------------------------------------
        //ingresando el id d eun usuairio trae solo las booking actuales --> ADMIN
        public List<BookingDetailDto> GetCurrentReservedBookingsByUserId(int userId)
        {
            var userExists = _movieDbContext.Users.Any(u => u.Id == userId);

            if (!userExists)
            {
                Console.WriteLine($"Advertencia: Usuario con ID {userId} no encontrado.");
                return new List<BookingDetailDto>();
            }

            var currentDate = DateTime.Now;

            var currentReservedBookings = _movieDbContext.BookingDetails
                .Include(bd => bd.IdBookingNavigation)
                .Include(bd => bd.IdMovieNavigation)
                .Where(bd => bd.IdBookingNavigation.IdUser == userId &&
                             bd.State == BookingDetailState.Reserved &&
                             (bd.ReturnDate == null || bd.ReturnDate > currentDate))
                .Select(bd => new BookingDetailDto
                {
                    Id = bd.Id,
                    MovieTitle = bd.IdMovieNavigation.Title,
                    State = bd.State,
                    BookingDate = bd.BookingDate,
                    ReturnDate = bd.ReturnDate,
                    Comment = bd.Comment,
                })
                .ToList();

            return currentReservedBookings;
        }



        //---------------------------------------------------------------------------------------------------------------------------------
        //ingrensaod el id de la resevra modifica su estado por retornada o cancelada

        public bool UpdateBookingDetailState(int bookingDetailId, BookingDetailState newState)
        {
            var bookingDetail = _movieDbContext.BookingDetails
                .Include(bd => bd.IdBookingNavigation)
                .Include(bd => bd.IdMovieNavigation)
                .SingleOrDefault(bd => bd.Id == bookingDetailId &&
                                       bd.State == BookingDetailState.Reserved); // Puedes ajustar la condición según tus necesidades

            if (bookingDetail != null)
            {
                bookingDetail.State = newState;
                _movieDbContext.SaveChanges();
                return true; // Indica que se actualizó correctamente
            }
            else
            {
                return false; // Indica que no se encontró la reserva con el ID proporcionado o no cumplió con las condiciones
            }
        }


    }
}






//-----------------------------------------------------------------------
//verifica si el usuario existe, no existe o no tiene reservas asociadas y devuelve su booking id    -> ADMIN
/*
public BookingResult CheckBookingByIdUser(int userId)
{
    // Verifica si el usuario existe
    var userExists = _movieDbContext.Users.Any(u => u.Id == userId);

    if (!userExists)
    {
        // El usuario no existe, puedes manejar este caso según tus necesidades.
        // Puedes devolver un resultado indicando que el usuario no existe.
        return new BookingResult
        {
            Success = false,
            Message = $"El usuario con ID {userId} no existe.",
            BookingId = 0
        };
    }

    // Verifica si el usuario ya tiene una reserva existente
    var existingBooking = _movieDbContext.Bookings.FirstOrDefault(b => b.IdUser == userId);

    if (existingBooking == null)
    {
        // Si el usuario no tiene una reserva, devuelve un resultado indicando que no tiene reservas y sugerir que debe crear una
        return new BookingResult
        {
            Success = true,
            Message = "El usuario no tiene ninguna reserva existente. Se sugiere crear una reserva.",
            BookingId = 0
        };
    }
    else
    {
        // Si el usuario ya tiene una reserva, devuelve un resultado indicando la existencia de la reserva
        return new BookingResult
        {
            Success = true,
            Message = "El usuario ya tiene una reserva existente.",
            BookingId = existingBooking.Id
        };
    }
}
*/