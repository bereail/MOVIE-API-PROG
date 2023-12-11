using System.Runtime.Serialization;

namespace MOVIE_API.Models.Enum
{
    public enum BookingDetailState
    {
        [EnumMember(Value = "Reserved")]
        Reserved = 1,

        [EnumMember(Value = "Returned")]
        Returned = 2,        

        [EnumMember(Value = "Canceled")]
        Canceled = 3,

    }
}
