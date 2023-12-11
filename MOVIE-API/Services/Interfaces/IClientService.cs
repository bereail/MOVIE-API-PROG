using Microsoft.AspNetCore.Mvc;
using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;

namespace MOVIE_API.Services.Interfaces
{
    public interface IClientService
    {

        //funcion para crear un nuevo cliente
        int CreateClient(ClientCreateDto clientDto);
     
    }
}

