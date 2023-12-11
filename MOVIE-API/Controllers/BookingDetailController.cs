using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // Para StatusCodes
using Microsoft.AspNetCore.Mvc;
using movie_api.Services.Interfaces;
using MOVIE_API.Models.DTO;
using MOVIE_API.Services.Interfaces;

namespace MOVIE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IUserService _userService;

        public BookingDetailController(IBookingDetailService bookingDetailService, IUserService userService)
        {
            _bookingDetailService = bookingDetailService;
            _userService = userService;
        }


        //-----------------------------------------------------------------------------------------------------------
        //CreatedResult un nueva reserva
        //solo se pueda crear una reserva con una movie de state = dispobible (1)
        // si el pelicula no existe ono est adispobible envia un msj de  ade


        [HttpPost("addDetails")]
        [Authorize] // Asegura que solo usuarios autenticados puedan acceder a este endpoint
        public IActionResult AddBookingDetails([FromBody] BookDetailsPostDto details)
        {
            try
            {
                // Obtén el ID de usuario del token de autenticación
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("No se pudo obtener el ID de usuario del token de autenticación");
                }

                // Ahora, puedes utilizar userId en tu lógica para asociar la reserva con el usuario correspondiente

                if (details == null)
                {
                    return BadRequest("Datos de detalles de reserva no proporcionados");
                }

                // Agregar más validaciones según sea necesario

                DateTime? returnDate = _bookingDetailService.AddBookingDetails(details, userId);

                if (returnDate.HasValue)
                {
                    // Devolver un mensaje con la fecha de retorno
                    return StatusCode(StatusCodes.Status201Created, new { Message = $"Detalles de reserva creados exitosamente. La fecha de retorno es {returnDate}" });
                }
                else
                {
                    // Manejar el caso en el que no se pudo crear la reserva
                    return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo crear la reserva");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Manejar excepciones específicas, puedes verificar el mensaje y proporcionar mensajes más específicos
                return BadRequest($"Error en la solicitud: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


    }
}
