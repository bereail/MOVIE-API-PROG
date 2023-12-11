using Microsoft.AspNetCore.Mvc;
using movie_api.Models;
using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;

namespace movie_api.Services.Interfaces
{
    public interface IMovieService
    {


        // trae solo todas las peliculs disponibles -> cualquier persona puede ver todas las peliculas
        List<MovieDto> GetAvailableMovies();


        // trae todas las peliculs y las ordena según su state
        List<MovieDto> GetMovies();

        //funcion para crear una pelicula
        int CreateMovie(MoviePostDto moviePostDto);


        // Modifica el estado de la película 
        IActionResult UpdateMovieState(int movieId, MovieState newState);


        //buscar pelicula por id
        MovieAndStateDto? GetMovieById(int movieId);


        //busca un pelicula según su title
        List<MovieDto> SearchMoviesByTitle(string title);

       
    }
}
