
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_api.Models.DTO;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using System;
using System.Diagnostics;
using System.Linq;

namespace movie_api.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly moviedbContext _moviedbContext;

        public MovieService(moviedbContext moviedbContext)
        {
            _moviedbContext = moviedbContext;
        }

      



        //------------------------------------------------------------------------------------------------------------

        // trae todas las peliculs disponibles -> cualquier persona puede ver todas las peliculas
        // Método que trae todas las películas disponibles
        public List<MovieDto> GetAvailableMovies()
        {
            try
            {
                var moviesDtos = _moviedbContext.Movies
                    .Where(movie => movie.State == MovieState.Available)
                    .Select(MapToDto)
                    .ToList();

                return moviesDtos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al obtener películas disponibles: {ex.Message}");
                return new List<MovieDto>();
            }
        }

        private MovieDto MapToDto(Movie movie)
        {
            return new MovieDto
            {
                Title = movie.Title,
                Director = movie.Director,
                // No incluir la propiedad State en el DTO
            };
        }


        //-----------------------------------------------------------------------------------------------------------------------

        // trae todas las peliculs y las ordena según su state -> solo el ADMIN puede ser las pelicula no disponibles o eliminadas
        // 2opcion, todos los usuair pueden ver las peliculas pero se ordenadn en dispobibles o no dispobles en caso que quieran alquilar una 
        //pelicula pero que no se encuntre dispobible en el momento (revisar fucnion)
        public List<MovieDto> GetMovies()
        {
            try
            {
                var moviesDtos = _moviedbContext.Movies
                    .Where(movie => movie.State == MovieState.Available)  // Filtro por estado "Available"
                    .Select(MapToDto)
                    .ToList();

                return moviesDtos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al obtener películas disponibles: {ex.Message}");
                return new List<MovieDto>();
            }
        }






        //------------------------------------------------------------------------------------------------------------


        // Función para crear una película -> ADMIN
        public int CreateMovie(MoviePostDto moviePostDto)
        {
            var newMovie = new Movie
            {
                Title = moviePostDto.Title,
                Director = moviePostDto.Director,
                Date = DateTime.Now, // Establecer manualmente la fecha actual
                State = MovieState.Available// Establecer manualmente el estado
            };

            _moviedbContext.Add(newMovie);
            _moviedbContext.SaveChanges();
            return newMovie.Id;
        }





        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Modifica el estado de una película -> ADMIN
        public IActionResult UpdateMovieState(int movieId, MovieState newState)
        {
            try
            {
                if (!IsValidMovieState(newState))
                {
                    return new BadRequestObjectResult($"El estado {newState} no es válido.");
                }

                var movie = _moviedbContext.Movies.Find(movieId);

                if (movie != null)
                {
                    movie.State = newState;
                    _moviedbContext.SaveChanges();
                    return new OkObjectResult($"Estado de la película con ID {movieId} actualizado exitosamente a {newState}");
                }

                return new NotFoundObjectResult($"No se encontró ninguna película con ID {movieId}");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        private bool IsValidMovieState(MovieState state)
        {
            return Enum.IsDefined(typeof(MovieState), state);
        }




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //busca un pelicula según su title -> cualquier usuario 
        // modicar para que encuente todas menos las eliminadas, por caso si quiera alquilar alguna que solo por ahora esta reservada
        //no es case sensitive
        public List<MovieDto> SearchMoviesByTitle(string title)
        {
            try
            {
                var moviesDtos = _moviedbContext.Movies
                    .Where(movie => movie.Title.ToLower().Contains(title.ToLower()))
                    .Select(MapToDto)
                    .ToList();

                return moviesDtos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al buscar películas por título: {ex.Message}");
                return new List<MovieDto>();
            }
        }




//--------------------------------------------------------------------------------------------------------------------

//Buscar pelicula por id -> ADMIN
// encuentra todas las peliculas entodos sus estados

public MovieAndStateDto GetMovieById(int movieId)
{
    try
    {
        var movie = _moviedbContext.Movies.Find(movieId);

        if (movie != null)
        {
            return new MovieAndStateDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                Date = movie.Date,
                State = movie.State,
            };
        }

        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al obtener película por ID: {ex.Message}");
        return null;
    }
}
    }

}




