using movie_api.Models.DTO;
using MOVIE_API.Models;
using MOVIE_API.Models.DTO;
using MOVIE_API.Models.Enum;
using MOVIE_API.Services.Interfaces;

namespace MOVIE_API.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly moviedbContext _moviedbContext;


        public AdminService(moviedbContext moviedbContext)
        {
            _moviedbContext = moviedbContext;
        }



        /*-------------------------------------------------------------------------------------------------------------*/

        //crear Admin y lo asocia a un nuevo booking id
        public int CreateAdmin(AdminCreateDto adminDto)
        {
            try
            {
                // Verificar si el usuario ya existe y no tiene un rol de administrador
                var existingUser = _moviedbContext.Users.SingleOrDefault(u => u.Email == adminDto.Email);

                if (existingUser == null)
                {
                    // Crear un nuevo objeto de tipo User y asignarle los valores del DTO
                    User newUser = new User
                    {
                        Name = adminDto.Name,
                        Lastname = adminDto.Lastname,
                        Email = adminDto.Email,
                        Pass = adminDto.Pass,
                        Rol = UserRole.Admin  // Asignar el rol de administrador (o el valor correspondiente)
                    };

                    // Crear un nuevo objeto de tipo Admin y asignarle los valores del DTO
                    Admin newAdmin = new Admin
                    {
                        EmployeeNum = adminDto.EmployeeNum
                    };

                    // Asociar el administrador al usuario
                    newUser.Admins.Add(newAdmin);

                    // Agregar el nuevo usuario a la base de datos
                    _moviedbContext.Users.Add(newUser);
                    _moviedbContext.SaveChanges();

                    // Crear una nueva reserva asociada al usuario recién creado
                    var newBooking = new Booking
                    {
                        IdUser = newUser.Id,
                        // Otras propiedades de la reserva (que no están especificadas en el código proporcionado)
                    };

                    // Agregar la nueva reserva a la base de datos
                    _moviedbContext.Bookings.Add(newBooking);
                    _moviedbContext.SaveChanges();

                    return newUser.Id;  // Devolver el Id del nuevo usuario
                }
                else
                {
                    // Usuario ya existe, manejar según tus requisitos
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear un administrador: {ex.Message}");
                throw;
            }
        }



        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        // trae todas las peliculs y las ordena según su state
        public List<MovieAndStateDto> GetMoviesAndState()
        {
            try
            {
                var moviesDtos = _moviedbContext.Movies
                    .Select(MapToDto)
                    .ToList();

                return moviesDtos;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al obtener películas: {ex.Message}");
                return new List<MovieAndStateDto>();
            }
        }
        private MovieAndStateDto MapToDto(Movie movie)
        {
            return new MovieAndStateDto
            {
                Id = movie.Id,
                IdAdmin = movie.IdAdmin,
                Title = movie.Title,
                Director = movie.Director,
                Date = movie.Date,
                State = movie.State
            };
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        //crea un adminstrador

       /* public int CreateAdmin(AdminCreateDto adminCreateDto)
        {
            try
            {
                var newAdmin = new Admin
                {
                    EmployeeNum = adminCreateDto.EmployeeNum,
                    // Otros campos necesarios para crear un administrador
                    IdUser = adminCreateDto.IdPerson, // <-- Ajusta esta línea según tus necesidades
                    Rol = UserRole.Admin  // Establece el rol por defecto como "Admin"
                };

                _moviedbContext.Admins.Add(newAdmin);
                _moviedbContext.SaveChanges();

                return newAdmin.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el administrador: {ex.Message}");
                throw new ApplicationException($"Error al crear el administrador: {ex.Message}");
            }
        }
    }*/

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /*//trae todos los adminsitradores
        public List<AdminDto> GetAdmins()
                {
                    try
                    {
                        var admins = _moviedbContext.Admins.ToList();
                        var adminDtos = admins.Select(MapToDto).ToList();
                        return adminDtos;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener administradores: {ex.Message}");
                        return new List<AdminDto>();
                    }
                }*/

        //--------------------------------------------------------------------------------------------------------------------------------------------------------


        // Otros métodos para actualizar, eliminar y obtener administradores por ID, si es necesario

        /*  public AdminDto MapToDto(Admin admin)
                  {
                      // Mapeo de entidad Admin a DTO
                      return new AdminDto
                      {
                          Id = admin.Id,
                          EmployeeNum = admin.EmployeeNum,
                          // Otros campos necesarios
                      };
                  }*/
    }
}

