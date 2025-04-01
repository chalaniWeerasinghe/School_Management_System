using System.ComponentModel.DataAnnotations;
using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class Teacher
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

