using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Services.Interfaces;
using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using MOVIE_API.Models.Enum;
using movie_api.Services.Implementations;

namespace MOVIE_API.Controllers
{
    [Authorize] //todos estos endpoint estan protegidos 
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------
        //trae un usuario por su id 

        [HttpGet("getUserById/{id}")]
        [Authorize]
        public IActionResult GetUserById(int id)
        {
            try
            {
                //  obtener el usuario por ID
                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    // Devuelve código de estado 404 Not Found si el usuario no se encuentra
                    return NotFound($"No se encontró ningún usuario con ID {id}");
                }

                // Devuelve el usuario encontrado 
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Maneja otras excepciones y devuelves un código de estado 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        //funcion para traer todos los usuarios 

        [HttpGet("users")]

        public IActionResult GetUsers()
        {
            try
            {
                var users = _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener personas: {ex.Message}");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        // Función para obtener todos los administradores 

        [HttpGet("getAdmins")]
        public IActionResult GetAdmins()
        {
            try
            {
                var admins = _userService.GetAdmins();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener administradores: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        //funcion para traer a todos los clientes 
        [HttpGet("getClients")]
        public IActionResult GetClients()
        {
            try
            {
                var clients = _userService.GetClients();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }

        }
    }
}


