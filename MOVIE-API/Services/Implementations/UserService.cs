using System;
using System.Collections.Generic;
using System.Linq;
using MOVIE_API.Models;
using Microsoft.EntityFrameworkCore;
using movie_api.Services.Interfaces;
using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;

namespace movie_api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly moviedbContext _movieDbContext;

        public UserService(moviedbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------

        //busca un usuario por su email
        public User? GetUserByEmail(string email)
        {
            return _movieDbContext.Users.SingleOrDefault(u => u.Email == email);


        }



        //-----------------------------------------------------------------------------------------------------------------
        //login


        public BaseResponse Login(string email, string password)
        {
            BaseResponse response = new BaseResponse();
            //User? userForLogin = _consultaContext.Users.SingleOrDefault(u => u.Email == email);
            User? userForLogin = GetUserByEmail(email);
            //if (string.IsNullOrEmpty(userForLogin.UserName) || string.IsNullOrEmpty(userForLogin.Password))
            //    return null;


            if (userForLogin != null)
            {
                if (userForLogin.Pass == password)
                {
                    response.Result = true;
                    response.Message = "loging Succesfull";
                }
                else
                {
                    response.Result = false;
                    response.Message = "wrong password";
                }
            }
            else
            {
                response.Result = false;
                response.Message = "wrong email";
            }


            return response;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------

        //trae un usuario por su id
        public User? GetUserById(int id)
        {
            return _movieDbContext.Users.SingleOrDefault(u => u.Id == id);
        }




        //----------------------------------------------------------------------------------------------------------------------------------------
        // Función para obtener todas los usuarios registrados  

        public List<UserDto> GetUsers()
        {
            try
            {
                var users = _movieDbContext.Users.ToList();
                var usersDtos = users.Select(MapToDto).ToList();
                return usersDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                return new List<UserDto>();
            }
        }

        // Mapeo de entidad Person a DTO
        public UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Rol = user.Rol, 
            };
        }



        //----------------------------------------------------------------------------------------------------------------------------------------
        // Función para obtener todos los administradores 

        public IEnumerable<AdminDto> GetAdmins()
        {
            try
            {
                var admins = _movieDbContext.Users
                .Where(u => u.Rol == UserRole.Admin)
            .ToList();
                var adminDtos = admins.Select(MapToAdminDto).ToList();
                return adminDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener administradores: {ex.Message}");
                return Enumerable.Empty<AdminDto>();
            }
        }

        private AdminDto MapToAdminDto(User user)
        {
            return new AdminDto
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email
            };
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        //funcion para traer a todos los clientes 

        public IEnumerable<ClientDto> GetClients()
        {
            try
            {
                // Obtener todos los usuarios con el rol "Client" desde la base de datos
                var clients = _movieDbContext.Users.Where(u => u.Rol == UserRole.Client).ToList();

                // Mapear los usuarios a objetos ClientDto usando la función MapToClientDto
                var clientDtos = clients.Select(MapToClientDto).ToList();

                // Devolver la lista de ClientDto
                return clientDtos;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la obtención de clientes
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");

                // En caso de error, devolver una lista vacía de ClientDto
                return Enumerable.Empty<ClientDto>();
            }
        }

        // Función para mapear un objeto User a un objeto ClientDto
        private ClientDto MapToClientDto(User user)
        {
            // Crear y devolver un nuevo objeto ClientDto con propiedades mapeadas desde el objeto User
            return new ClientDto
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email
            };
        }



    }
}

