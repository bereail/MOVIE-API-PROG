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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;


        public UserController(IUserService userService, IMovieService movieService)
        {
            _userService = userService;
            _movieService = movieService;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------
        //trae un usuario por su id -> ADMIN
        //modificar el DTO que devuelve

        [HttpGet("getUserById/{id}")]
        [Authorize]
        public IActionResult GetUserById(int id)
        {
            try
            {
                // Llamas al servicio para obtener el usuario por ID
                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    // Devuelves un código de estado 404 Not Found si el usuario no se encuentra
                    return NotFound($"No se encontró ningún usuario con ID {id}");
                }

                // Devuelves el usuario encontrado en el cuerpo de la respuesta
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Manejas otras excepciones y devuelves un código de estado 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }








        //------------------------------------------------------------------------------------------------------------------------------------------



        //funcion para traer todos los usuarios -> ADMIN
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
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




        //--------------------------------------------------------------------------------------------------------------------
        //trae un usuario por su id  -> ADMIN


        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userService.GetUserById(userId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"No se encontrador usuarios con ese id");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        // Función para obtener todos los administradores -> ADMIN 

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

        //funcion para traer a todos los clientes -> ADMIN
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


