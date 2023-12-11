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







        //-----------------------------------------------------------------------------------------------------------------
        //busca un usuario por su email
        //Buscar usuario por mail -----------------------------------------------------------------------------------
        public User? GetUserByEmail(string email)
        {
            return _movieDbContext.Users.SingleOrDefault(u => u.Email == email);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------





        //trae un usuario por su id
        public User? GetUserById(int id)
        {
            return _movieDbContext.Users.SingleOrDefault(u => u.Id == id);
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

            //if (userForLogin.UserType == "alumno")
            //        return userForLogin.FirstOrDefault(p => p.UserName == userForLogin.UserName && p.Password == userForLogin.Password);
            //    return userForLogin.FirstOrDefault(p => p.UserName == userForLogin.UserName && p.Password == userForLogin.Password);


            return response;
        }


      /*  revisar, estaba"duplicado" en movieService
       *  // validacion de credenciales - login -------------------------------------------------------------------------
        public BaseResponse Login(string email, string userPassword)
        {
            BaseResponse response = new BaseResponse();
            User? userForLogin = _moviedbContext.Users.SingleOrDefault(u => u.Email == email);
            if (userForLogin != null)
            {
                if (userForLogin.Pass == userPassword)
                {
                    response.IsSuccess = true;
                    response.Message = "loging Succesfull";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "wrong password";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "wrong email";
            }
            return response;
        }


        */




        //----------------------------------------------------------------------------------------------------------------------------------------




        // Función para obtener todas los usuarios registrados  -> funcion del admin o superadmin
        //verificar el mapeo de datos, mejorar el dto
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
                Rol = user.Rol, // Convertir UserRole a int directamente
            };
        }



        //----------------------------------------------------------------------------------------------------------------------------------------
        // Función para obtener todos los administradores -> ADMIN 

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
                // Puedes mapear más propiedades si es necesario
            };
        }

//-----------------------------------------------------------------------------------------------------------------------------

        //funcion para traer a todos los clientes -> ADMIN

        public IEnumerable<ClientDto> GetClients()
        {
            try
            {
                var clients = _movieDbContext.Users.Where(u => u.Rol == UserRole.Client).ToList();
                var clientDtos = clients.Select(MapToClientDto).ToList();
                return clientDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return Enumerable.Empty<ClientDto>();
            }
        }



        private ClientDto MapToClientDto(User user)
        {
            return new ClientDto
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email
                // Puedes mapear más propiedades si es necesario
            };
        }


    }
}

