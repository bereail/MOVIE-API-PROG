using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using movie_api.Models.DTO;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MOVIE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthenticateController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _config = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody] CredentialsDto credentialsDto)
        {
            // Paso 1: Validamos las credenciales
            if (credentialsDto == null)
            {
                return BadRequest("Las credenciales no pueden ser nulas.");
            }

            BaseResponse validarUsuarioResult = _userService.Login(credentialsDto.Email, credentialsDto.Pass);

            if (validarUsuarioResult.Message == "wrong email")
            {
                return BadRequest("Correo electrónico incorrecto");
            }
            else if (validarUsuarioResult.Message == "wrong password")
            {
                return Unauthorized("Contraseña incorrecta");
            }

            if (validarUsuarioResult.Result)
            {
                User user = _userService.GetUserByEmail(credentialsDto.Email)!;

                var secretForKey = _config["JwtSettings:SecretForKey"];
                if (string.IsNullOrEmpty(secretForKey))
                {
                    return BadRequest("JwtSettings:SecretForKey no está configurado correctamente.");
                }

                var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretForKey));
                var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

                var claimsForToken = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("email", user.Email)
        };

                // No es necesario verificar si user.Rol es nulo, ya que UserRole no es nullable
                claimsForToken.Add(new Claim("rol", user.Rol.ToString()));

                var jwtSecurityToken = new JwtSecurityToken(
                    _config["JwtSettings:Issuer"],
                    _config["JwtSettings:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    credentials);

                string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);
            }

            return BadRequest("Falló la autenticación");
        }

    }
}