using MOVIE_API.Models.DTO;
using MOVIE_API.Models;
using MOVIE_API.Services.Interfaces;
using MOVIE_API.Models.Enum;

namespace MOVIE_API.Services.Implementations
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly moviedbContext _movieDbContext;

        public BookingDetailService(moviedbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //crear una nueva reserva  ->AddBookingDetails
        public DateTime? AddBookingDetails(BookDetailsPostDto details, string userId)
        {
            try
            {

                int? bookingId = GetBookingIdByUserId(userId);

                if (!bookingId.HasValue)
                {
                    throw new InvalidOperationException($"No se encontró un BookingId para el usuario con ID {userId}.");
                }

                var movie = _movieDbContext.Movies.Find(details.IdMovie);

                if (movie == null)
                {
                    throw new InvalidOperationException($"La película con ID {details.IdMovie} no existe.");
                }

                // Verificar si la película tiene el estado correcto (por ejemplo, State = Available)
                var availableMovie = _movieDbContext.Movies.SingleOrDefault(m => m.Id == details.IdMovie && m.State == MovieState.Available);

                if (availableMovie == null)
                {
                    throw new InvalidOperationException($"La película con ID {details.IdMovie} no está disponible actualmente.");
                }


                var bookingDetail = new BookingDetail
                {
                    IdMovie = details.IdMovie,
                    Comment = details.Comment,
                    IdBooking = bookingId.Value
                };

                bookingDetail.BookingDate = DateTime.Now;
                bookingDetail.ReturnDate = DateTime.Now.AddHours(48);
                bookingDetail.State = BookingDetailState.Reserved;

              

                _movieDbContext.BookingDetails.Add(bookingDetail);
                _movieDbContext.SaveChanges();

               

                SendNotification(bookingDetail);


                return bookingDetail.ReturnDate;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error en la reserva: {ex.Message}");
                throw; // Re-lanza la excepción para que pueda ser manejada específicamente en el controlador
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general en la reserva: {ex.Message}");
                return null;
            }
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //obtiene un usuario por su id
        public int? GetBookingIdByUserId(string userId)
        {
            return _movieDbContext.Bookings
                .Where(booking => booking.IdUserNavigation.Id.ToString() == userId)
                .Select(booking => booking.Id)
                .FirstOrDefault();
        }

        private void SendNotification(BookingDetail bookingDetail)
        {
            // Implementa la lógica de notificación según sea necesario
            Console.WriteLine($"Se ha creado una nueva reserva. La fecha de devolución es {bookingDetail.ReturnDate}.");
        }
    }
}