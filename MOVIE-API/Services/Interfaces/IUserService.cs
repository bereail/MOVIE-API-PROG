using movie_api.Models.DTO;
using MOVIE_API.Models;
using MOVIE_API.Models.DTO;
using System.Collections.Generic;

namespace movie_api.Services.Interfaces
{
    public interface IUserService
    {

        //busca un usuario por su email -> funcion para el login
        public User? GetUserByEmail(string email);

        //funcion para logearse
        public BaseResponse Login(string mail, string password);

        //busca un usuario por su id
        User? GetUserById(int id);


        //funcion para traer todas los usuarios registrados
        List<UserDto> GetUsers();


        //traer todos los admins
        public IEnumerable<AdminDto> GetAdmins();

        //trar todos los clientes
        public IEnumerable<ClientDto> GetClients();





    }
}

