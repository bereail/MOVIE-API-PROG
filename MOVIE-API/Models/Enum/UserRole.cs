using System.ComponentModel.DataAnnotations;

namespace MOVIE_API.Models.Enum
{
    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Client")]
        Client = 2
    }
}
