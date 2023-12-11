
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
using System.Security.Claims;

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
        //// trae todas las reservas asociadas a un userId y la ordena segun su estado actuales o historial
        [HttpGet("bookingsAndDetails")]
        [Authorize] // Asegura que el usuario esté autenticado
        public IActionResult GetBookingsAndDetails()
        {
            try
            {
                // Obtiene el userId del usuario autenticado
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Llama al servicio para obtener los detalles de las reservas por usuario
                var bookingsAndDetails = _bookingService.GetBookingsAndDetailsByUserId(userId);

                if (bookingsAndDetails.Count > 0)
                {
                    // Filtra las reservas actuales basándose en el estado
                    var currentBookings = bookingsAndDetails
                        .Where(bd => bd.State == BookingDetailState.Reserved)
                        .ToList();

                    // Filtra las reservas históricas basándose en el estado
                    var historicalBookings = bookingsAndDetails
                        .Where(bd => bd.State != BookingDetailState.Reserved)
                        .ToList();

                    // Devuelve un objeto JSON con las reservas actuales e históricas
                    return Ok(new { CurrentBookings = currentBookings, HistoricalBookings = historicalBookings });
                }

                // Devuelve un mensaje si no se encuentran reservas para el usuario
                return NotFound($"No se encontraron reservas para el usuario con ID {userId}");
            }
            catch (Exception ex)
            {
                // Devuelve un código de estado 500 y un mensaje de error si hay una excepción
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener reservas: {ex.Message}");
            }
        }



        //---------------------------------------------------------------------------------------------------------------------------
        ////crea un nueva booking detail asociada a un id user
        [HttpPost("CreateBookingDetail")]
        [Authorize] // Asegura que el usuario esté autenticado
        public IActionResult CreateBookingDetail([FromBody] BookingDetailPostDto bookingDetailDto)
        {
            try
            {
                // Obtiene el userId del usuario autenticado
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Llama al servicio para crear el BookingDetail
                var result = _bookingService.CreateBookingDetail(userId, bookingDetailDto);

                // Verifica si la operación fue exitosa
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                // Devuelve un código de estado 500 y un mensaje de error si hay una excepción
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la reserva: {ex.Message}");
            }
        }




        //---------------------------------------------------------------------------------------------------------------------------

        //trae todas las reservas actuales de un usuairo ingrensado su id
        [HttpGet("bookingsAndDetails/{userId}")]
        public IActionResult GetBookingsAndDetailsByUserId(int userId)
        {
            try
            {
                var bookingsAndDetails = _bookingService.GetBookingsAndDetailsByUserIdFromFrontend(userId);

                if (bookingsAndDetails.Count > 0)
                {
                    var currentBookings = bookingsAndDetails
                        .Where(bd => bd.State == BookingDetailState.Reserved)
                        .ToList();

                    var historicalBookings = bookingsAndDetails
                        .Where(bd => bd.State != BookingDetailState.Reserved)
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


