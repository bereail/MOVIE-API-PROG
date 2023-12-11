
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Models.DTO;
using movie_api.Services.Interfaces;
using MOVIE_API.Models.DTO;
using System.Text.Json.Serialization;
using System.Text.Json;
using MOVIE_API.Models;
using movie_api.Services;
using Microsoft.AspNetCore.Authorization;
using MOVIE_API.Models.Enum;

namespace movie_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }




        //------------------------------------------------------------------------------------------------------------
        //// Verifica si el usuario tiene reservas y las clasifica en actuales e historial
        //mejorar manejor d errores para verificar si el id existe, no tiene reservas o no existe
        //mejorar la agrupacion, no las diferencia por su state
        //funcion para el ADMIN

        [HttpGet("bookingsAndDetails/{userId}")]
        public IActionResult GetBookingsAndDetailsByUserId(int userId)
        {
            try
            {
                var bookingsAndDetails = _bookingService.GetBookingsAndDetailsByUserId(userId);

                if (bookingsAndDetails.Count > 0)
                {
                    var currentBookings = bookingsAndDetails
                        .Where(bd => bd.ReturnDate == null || bd.ReturnDate > DateTime.Now)
                        .ToList();

                    var historicalBookings = bookingsAndDetails
                        .Where(bd => bd.ReturnDate != null && bd.ReturnDate <= DateTime.Now)
                        .ToList();

                    return Ok(new { CurrentBookings = currentBookings, HistoricalBookings = historicalBookings });
                }

                return NotFound($"No se encontraron reservas para el usuario con ID {userId}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener reservas: {ex.Message}");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------
        //revisar para no poder ingresar id por consola sino cunado se logea, y no poder crear resver con una mnovie de state que no sea 1

        ////crea un nueva booking detail asociada a un id user
        [HttpPost("createBookingDetail/{userId}")]
        public IActionResult CreateBookingDetail(int userId, [FromBody] BookingDetailPostDto bookingDetailDto)
        {
            try
            {
                var result = _bookingService.CreateBookingDetail(userId, bookingDetailDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error al crear el BookingDetail: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el BookingDetail: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }

        }




        //---------------------------------------------------------------------------------------------------------------------------



        //trae las booking_details asociadas a un id de user  (MODIFICAR PARA QUE SOLO TRAIGA LA BOOKING ACTUALES STATE = 1)

        [HttpGet("api/User/{userId}/BookingsAndDetails")]
        public IActionResult GetBookingDetailsByUserId(int userId)
        {
            try
            {
                List<BookingDetailDto> bookingDetails = _bookingService.GetBookingsAndDetailsByUserId(userId);

                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                string jsonResult = JsonSerializer.Serialize(bookingDetails, jsonOptions);

                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }






        //----------------------------------------------------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------------------------------------------
        //ingresando el id d eun usuairio trae solo las booking actuales  

        [HttpGet("currentReservedBookings/{userId}")]
        public IActionResult GetCurrentReservedBookingsByUserId(int userId)
        {
            try
            {
                var currentReservedBookings = _bookingService.GetCurrentReservedBookingsByUserId(userId);

                if (currentReservedBookings.Any())
                {
                    return Ok(currentReservedBookings);
                }
                else
                {
                    return NotFound($"No se encontraron reservas actuales para el usuario con ID {userId}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        //---------------------------------------------------------------------------------------------------------------------------------

        //ingrensaod el id de la resevra modifica su estado por retornada o cancelada
        [HttpPut("updateBookingState/{bookingDetailId}")]
        public IActionResult UpdateBookingState(int bookingDetailId, [FromBody] BookingDetailStateDto bookingDetailStateDto)
        {
            try
            {
                // Validar que el valor de NewStateName sea "Reserved", "Returned" o "Canceled"
                if (bookingDetailStateDto.NewStateName != "Reserved" && bookingDetailStateDto.NewStateName != "Returned" && bookingDetailStateDto.NewStateName != "Canceled")
                {
                    return BadRequest("El valor de NewStateName debe ser 'Reserved', 'Returned' o 'Canceled'.");
                }

                // Mapear el nombre del estado a un valor de enumeración
                var newState = Enum.Parse<BookingDetailState>(bookingDetailStateDto.NewStateName);

                // Llamar al método de servicio con el nuevo estado
                var updateResult = _bookingService.UpdateBookingDetailState(bookingDetailId, newState);

                if (updateResult)
                {
                    return Ok($"Estado de reserva actualizado correctamente a {bookingDetailStateDto.NewStateName}.");
                }
                else
                {
                    return NotFound($"No se encontró la reserva con ID {bookingDetailId} o no cumplió con las condiciones para la actualización.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

    }

}





//---------------------------------------------------------------------------------------------------------------------------






//--------------------------------------------------------------------------------------------------------------------------------------
//verifica si el usuario existe, no existe o no tiene reservas asociadas y devuelve su booking id
/*[HttpGet("CheckBooking/{userId}")]
public IActionResult CheckBooking(int userId)
{
    try
    {
        var result = _bookingService.CheckBookingByIdUser(userId);

        if (result.Success)
        {
            return Ok(new { Message = result.Message, BookingId = result.BookingId });
        }
        else
        {
            return BadRequest(new { Message = result.Message });
        }
    }
    catch (Exception ex)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
    }
}
*/