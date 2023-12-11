using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using movie_api.Services;
using movie_api.Services.Implementations;
using movie_api.Services.Interfaces;
using MOVIE_API.Models;
using MOVIE_API.Services.Implementations;
using MOVIE_API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.VisualStudio.Services.Users;




var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<moviedbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
             new List<string>()
        }
    });
}); ;



// Add DbContext
builder.Services.AddDbContext<moviedbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add other services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingDetailService, BookingDetailService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IClientService, ClientService>();





// Configuracion de Token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        RoleClaimType = ClaimTypes.Role,
        ValidAudience = "movieapi",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretForKey"]))
    };
});


// Politicas de autorizacion --------------------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("rol", "Admin"));
    options.AddPolicy("Client", policy => policy.RequireClaim("rol", "Client"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    var user = context.User;
    if (user.Identity.IsAuthenticated)
    {
        Console.WriteLine($"Authenticated User: {user.Identity.Name}");
        Console.WriteLine("User Claims:");
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        Console.WriteLine($"Role Claim: {roleClaim?.Value}");
    }

    await next();
});
app.UseAuthorization();

app.MapControllers();

app.Run();