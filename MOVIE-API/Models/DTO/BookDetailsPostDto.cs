using MOVIE_API.Models;
using System;
using System.Collections.Generic;

namespace MOVIE_API.Models.DTO
{
    public class BookDetailsPostDto 
    {
        public int? IdMovie { get; set; }
        public string Comment { get; set; }
    }
}


//-----------------------------------------------------------------------------------------------------------
//CreatedResult un nueva reserva