
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

        // trae todas las peliculs disponibles 
        public List<MovieDto> GetAvailableMovies()
        {
            try
            {
                // Obtener una lista de películas disponibles desde la base de datos
                var moviesDtos = _moviedbContext.Movies
                    .Where(movie => movie.State == MovieState.Available)
                    .Select(MapToDto)
                    .ToList();

                // Devolver la lista de objetos MovieDto
                return moviesDtos;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la obtención de películas disponibles
                Console.WriteLine($"Error al obtener películas disponibles: {ex.Message}");

                // En caso de error, devolver una lista vacía de MovieDto
                return new List<MovieDto>();
            }
        }

        //  mapea un objeto Movie a un objeto MovieDto
        private MovieDto MapToDto(Movie movie)
        {
            // Crear y devolver un nuevo objeto MovieDto con propiedades mapeadas desde el objeto Movie
            return new MovieDto
            {
                Title = movie.Title,
                Director = movie.Director,
                State = movie.State
            };
        }

        //------------------------------------------------------------------------------------------------------------
        // Función para crear una película 
        public int CreateMovie(MoviePostDto moviePostDto)
        {
            var newMovie = new Movie
            {
                Title = moviePostDto.Title,
                Director = moviePostDto.Director,
                Date = DateTime.Now, // Establece por default la fecha actual
                State = MovieState.Available// Se establece disponible por default
            };

            _moviedbContext.Add(newMovie);
            _moviedbContext.SaveChanges();
            return newMovie.Id;
        }




        //-----------------------------------------------------------------------------------------------------------------------

        // trae TODAS las peliculs y las ordena según su state 
        // Diccionario almacena pares clave-valor, tiene MovieState como clave y List<MovieDto> como valor.
        public Dictionary<MovieState, List<MovieDto>> GetMoviesGroupedByState()
        {
            try
            {
                // Obtén todas las películas desde la base de datos y conviértelas a DTOs
                var moviesDtos = _moviedbContext.Movies
                    .Select(MapToDto)
                    .ToList();

                // Agrupa las películas por estado y las ordena descendientemente
                var groupedMovies = moviesDtos.GroupBy(movie => movie.State)
                    .ToDictionary(
                        group => group.Key,// La clave del diccionario es el estado
                        group => group.OrderByDescending(movie => movie.State).ToList()//// Ordena las películas dentro de cada grupo por estado de manera descendente
                    );
                // Devuelve el diccionario resultante
                return groupedMovies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener películas agrupadas por estado: {ex.Message}");
                return new Dictionary<MovieState, List<MovieDto>>();
            }
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Método para actualizar el estado de una película
        public IActionResult UpdateMovieState(int movieId, MovieState newState)
        {
            try
            {
                // Verifica si el nuevo estado es válido usando el método IsValidMovieState
                if (!IsValidMovieState(newState))
                {
                    // Devuelve una respuesta de error si el estado no es válido
                    return new BadRequestObjectResult($"El estado {newState} no es válido.");
                }

                // Busca la película en la base de datos por su ID
                var movie = _moviedbContext.Movies.Find(movieId);

                // Si la película existe
                if (movie != null)
                {
                    // Actualiza el estado de la película al nuevo estado proporcionado
                    movie.State = newState;

                    // Guarda los cambios en la base de datos
                    _moviedbContext.SaveChanges();

                    // Devuelve una respuesta exitosa con un mensaje indicando la actualización exitosa
                    return new OkObjectResult($"Estado de la película con ID {movieId} actualizado exitosamente a {newState}");
                }

                // Si la película no existe, devuelve una respuesta de error indicando que no se encontró
                return new NotFoundObjectResult($"No se encontró ninguna película con ID {movieId}");
            }
            // Captura cualquier excepción no manejada y devuelve un error interno del servidor
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Método para verificar si un estado de película es válido
        private bool IsValidMovieState(MovieState state)
        {
            // Utiliza Enum.IsDefined para verificar si el estado está definido en el enumerador MovieState
            return Enum.IsDefined(typeof(MovieState), state);
        }


//--------------------------------------------------------------------------------------------------------------------

//Buscar pelicula por id 
// encuentra todas las peliculas entodos sus estados

public MovieAndStateDto GetMovieById(int movieId)
        {
            try
            {
                // Busca la película en la base de datos por su ID
                var movie = _moviedbContext.Movies.Find(movieId);

                // Si la película existe
                if (movie != null)
                {
                    // Crea y devuelve un objeto MovieAndStateDto con la información de la película
                    return new MovieAndStateDto
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Director = movie.Director,
                        Date = movie.Date,
                        State = movie.State,
                    };
                }

                // Si la película no existe, devuelve null
                return null;
            }
            // Captura cualquier excepción no manejada y muestra un mensaje de error en la consola
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener película por ID: {ex.Message}");
                return null;
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //busca un pelicula según su title -> cualquier usuario 
        //no es case sensitive
        //busca un pelicula según su title

        public List<MovieDto> SearchMoviesByTitle(string title)
        {
            try
            {
                var moviesDtos = _moviedbContext.Movies
                    .Where(movie => movie.State != MovieState.NotAvailable && movie.Title.ToLower().Contains(title.ToLower()))
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


    }

}




