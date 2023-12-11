using MOVIE_API.Models.DTO;

namespace MOVIE_API.Services.Interfaces
{
    public interface IBookingDetailService
    {
        DateTime? AddBookingDetails(BookDetailsPostDto details, string userId);

        int? GetBookingIdByUserId(string userId);
    }
}
