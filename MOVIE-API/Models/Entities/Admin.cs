﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MOVIE_API.Models;

public partial class Admin
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public string EmployeeNum { get; set; }

    public virtual User IdUserNavigation { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
