using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Models.DTO;
using movie_api.Services.Implementations;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using System;

namespace movie_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

//-----------------------------------------------------------------------------------------------------------------------------------
     // trae todas las peliculas dispobles (State = "Available" = 1)

        [HttpGet("GetAllAvailable")]
        public IActionResult GetAvailableMovies()
        {
            try
            {
                var movies = _movieService.GetAvailableMovies();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener películas: {ex.Message}");
            }
        }


//--------------------------------------------------------------------------------------------------------------------------
        //funcion para agregar una nueva pelicula -

        [HttpPost("addMovie")]
        [Authorize]
        public IActionResult CreateMovie([FromBody] MoviePostDto movie)
        {
            try
            {
                // Verifica si el usuario está autenticado
                if (User.Identity.IsAuthenticated)
                {

                    return StatusCode(StatusCodes.Status201Created, _movieService.CreateMovie(movie));
                }
                else
                {
                    // Devuelve un código de estado 401 Unauthorized si el usuario no está autenticado
                    return StatusCode(StatusCodes.Status401Unauthorized, "Debe iniciar sesión para realizar esta acción.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la película: {ex.Message}");
            }
        }


//-----------------------------------------------------------------------------------------------------------------------
        // trae TODAS las peliculs y las ordena según su state 

        [HttpGet("getMoviesGroupedByState")]
        [Authorize]
        public IActionResult GetMoviesGroupedByState()
        {
            try
            {
                var groupedMovies = _movieService.GetMoviesGroupedByState();


                return Ok(groupedMovies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener películas agrupadas por estado: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }



//--------------------------------------------------------------------------------------------------------------------------
        //modifica el estado de una pelicula    
        //-solo permite estados validos


        [HttpPut("movies/{id}/updateState")]
        [Authorize]
        public IActionResult UpdateMovieState(int id, [FromBody] MovieState newState)
        {
            try
            {
                var result = _movieService.UpdateMovieState(id, newState);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el estado de la película: {ex.Message}");
            }
        }


      

//-------------------------------------------------------------------------------------------------------------------------------------
//buscar peloicula por id -> ADMIN
 //encuntra todas la speloculas, no importa su estado

        [HttpGet("movies/{id}")]
        [Authorize]
        public IActionResult GetMovieById(int id)
        {
            try
            {
                var movie = _movieService.GetMovieById(id);

                if (movie != null)
                {
                    return Ok(movie);
                }

                return NotFound($"No se encontró ninguna película con ID {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener película por ID: {ex.Message}");
            }
        }



//--------------------------------------------------------------------------------------------------------------------------
// Buscar películas por título

        [HttpGet("searchByTitle")]
        public IActionResult SearchMoviesByTitle([FromQuery] string title)
        {
            try
            {
                var movies = _movieService.SearchMoviesByTitle(title);

                if (movies.Any())
                {
                    return Ok(movies);
                }

                return NotFound($"No se encontraron películas con el título '{title}'");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al buscar películas por título: {ex.Message}");
            }
        }
    }
}


