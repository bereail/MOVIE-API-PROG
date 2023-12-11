using System.ComponentModel.DataAnnotations;

namespace MOVIE_API.Models.Enum
{
    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin,

        [Display(Name = "Client")]
        Client
    }
}
