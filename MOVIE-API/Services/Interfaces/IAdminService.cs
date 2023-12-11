using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using System.Diagnostics;

namespace MOVIE_API.Services.Interfaces
{
    public interface IAdminService
    {
        //trae todas las peliculs y las ordena según su state
        List<MovieAndStateDto> GetMoviesAndState();


        //funcion para crear un admin
        int CreateAdmin(AdminCreateDto adminDto);


        // ver la opciones de liminar un cliente, solo borrandolo de la tabla cliente , ya que conservara, 
        //sus datos y reervas como user, pero no sera mas un cient
    }
}
