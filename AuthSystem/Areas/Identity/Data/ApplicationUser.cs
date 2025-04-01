using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Areas.Identity.Data;
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }
    public int Age => DateTime.Now.Year - BirthDate.Year; 
    public string? StudentNumber { get; set; }
    public string? EmployeeNumber { get; set; }

    //[PersonalData]
    //public DateTime? BirthDate { get; set; }

    //[PersonalData]
    //[Column(TypeName = "nvarchar(100)")]
    //public int? Age { get; set; }

    //[PersonalData]
    //[Column(TypeName = "nvarchar(100)")]
    //public string? StudentNo { get; set; }

    //[PersonalData]
    //public Gender? Gender { get; set; }
}

public enum Gender
{ 
    Male,
    Female
}
