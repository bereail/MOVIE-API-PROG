using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOVIE_API.Models.DTO;
using movie_api.Services.Interfaces;
using movie_api.Models.DTO;
using MOVIE_API.Models;
using MOVIE_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MOVIE_API.Models.Enum;


namespace MOVIE_API.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly moviedbContext _movieDbContext;

        public ClientService(moviedbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }
               
        //crea un nuevo cliente y lo asoocia a un nuevo booking id
        public int CreateClient(ClientCreateDto clientDto)
        {
                try
                {
                    // Verificar si el usuario ya existe y no tiene un rol de administrador
                    var existingUser = _movieDbContext.Users.SingleOrDefault(u => u.Email == clientDto.Email);

                    if (existingUser == null)
                    {
                        // Crear un nuevo objeto de tipo User y asignarle los valores del DTO
                        User newUser = new User
                        {
                            Name = clientDto.Name,
                            Lastname = clientDto.Lastname,
                            Email = clientDto.Email,
                            Pass = clientDto.Pass,
                            Rol = UserRole.Client
                        };

                    // Crear un nuevo objeto de tipo Client y asignarle los valores del DTO
                    Client newClient = new Client
                    {
                        
                    };
                    
                    // Asociar el cliente al usuario
                    newUser.Clients.Add(newClient);

                    // Agregar el nuevo usuario a la base de datos
                    _movieDbContext.Users.Add(newUser);
                        _movieDbContext.SaveChanges();

                        // Crear una nueva reserva asociada al usuario recién creado
                        var newBooking = new Booking
                        {
                            IdUser = newUser.Id,
                           
                        };

                        // Agregar la nueva reserva a la base de datos
                        _movieDbContext.Bookings.Add(newBooking);
                        _movieDbContext.SaveChanges();

                        return newUser.Id;  // Devolver el Id del nuevo usuario
                    }
                    else
                    {
                        // Usuario ya existe
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear un administrador: {ex.Message}");
                    throw;
                }
            }
        }
    }

   /*     //traer todos los clientes
        // Obtener clientes
        public List<PersonDto> GetClients()
        {
            try
            {
                var clients = _movieDbContext.Persons.Where(p => p.Rol == 2).ToList();
                var clientDtos = clients.Select(MapToDto).ToList(); // Utilizar un método de mapeo

                return clientDtos;
            }
            catch (Exception ex)
            {
               
                return new List<PersonDto>(); // Devolver una lista vacía en caso de error.
            }
        }

        // Método de mapeo para convertir un objeto Person a PersonDto
        private PersonDto MapToDto(Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Lastname = person.Lastname,
                Rol = person.Rol,
            };
        }


        // Crear cliente
        public int CreateClient(ClientPostDto clientDto)
        {
            try
            {
                // Crear una nueva persona (cliente)
                var clientId = _movieDbContext.Persons.Add(new Person
                {
                    Name = clientDto.Name,
                    Lastname = clientDto.Lastname,
                    Email = clientDto.Email,
                    Pass = clientDto.Pass,
                    Rol = 2, // Rol predefinido para cliente
                    // Otras propiedades...

                    // Asignar la entidad Booking si existe
                    Booking = clientDto.IdBooking.HasValue
                        ? new Booking { Id = clientDto.IdBooking.Value }
                        : null
                }).Entity.Id;

                _movieDbContext.SaveChanges();

                return clientId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
                throw;
            }
        }

        public List<BookingDto> GetClientBookings(int clientId)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(int clientId)
        {
            throw new NotImplementedException();
        }
    }
    //traer un cliente y sus reservas asociadas

    public List<BookingDto> GetClientBookings(int clientId)
        {
            try
            {
                var clientBookings = _movieDbContext.Bookings
                    .Where(b => b.Clients.Any(c => c.Id == clientId))
                    .Include(b => b.BookingDetails)
                    .Select(b => new BookingDto
                    {
                        Id = b.Id,
                        ReservationDate = b.ReservationDate,
                        BookingDetails = b.BookingDetails
                            .Select(d => new BookingDetailDto
                            {
                                Id = d.Id,
                                BookingState = d.BookingState,
                                Comments = d.Comments,
                                IdBooking = d.IdBooking,
                                IdMovie = d.IdMovie,
                                ReturnDate = d.ReturnDate
                                // Otras propiedades según sea necesario
                            })
                            .ToList()
                    })
                    .ToList();

                return clientBookings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener reservas del cliente: {ex.Message}");
                return new List<BookingDto>();
            }
        }

        //elimina el cliente y sus reservas asociadas
        public void DeleteClient(int clientId)
        {
            var client = _movieDbContext.Clients
                .Include(c => c.IdBookingNavigation)
                .SingleOrDefault(c => c.Id == clientId);

            if (client != null)
            {
                if (client.IdBookingNavigation != null)
                {
                    var booking = _movieDbContext.Bookings.Find(client.IdBookingNavigation.Id);
                    if (booking != null)
                    {
                        _movieDbContext.Bookings.Remove(booking);
                    }
                }

                _movieDbContext.Clients.Remove(client);
                _movieDbContext.SaveChanges();
            }
        }
    }
    }

*/