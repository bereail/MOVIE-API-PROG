using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using MOVIE_API.Models.Enum;
using MOVIE_API.Services.Interfaces;
using movie_api.Models.DTO;
using MOVIE_API.Models.DTO;
using System.Security.Claims;

namespace MOVIE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

//------------------------------------------------------------------------------------------------------------------


        // Trae todas las películas y las ordena según su estado
        [HttpGet("GetAllMoviesState")]
        /*[Authorize(Policy = "Admin")]*/
        public IActionResult GetAllMoviesState()
        {
            try
            {
                var movies = _adminService.GetMoviesAndState();

                var availableMovies = movies.Where(movie => movie.State == MovieState.Available).ToList();
                var reservedMovies = movies.Where(movie => movie.State == MovieState.Reserved).ToList();
                var notAvailableMovies = movies.Where(movie => movie.State == MovieState.NotAvailable).ToList();

                var result = new
                {
                    AvailableMovies = availableMovies,
                    ReservedMovies = reservedMovies,
                    NotAvailableMovies = notAvailableMovies
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener películas: {ex.Message}");
            }
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //crea un adminstrador
        
        [HttpPost("CreateAdmin")]
        public IActionResult CreateAdmin([FromBody] AdminCreateDto adminCreateDto)
        {
            try
            {
                _adminService.CreateAdmin(adminCreateDto);
                return Ok(new { Message = "Admin created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error creating admin: {ex.Message}" });
            }
        }
        



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //trae todos los admin
     /*   [HttpGet("admins")]
        public IActionResult GetAdmins()
        {
            try
            {
                var admins = _userService.GetAdmins();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener administradores: {ex.Message}");
            }
        }*/
    }
}

    
