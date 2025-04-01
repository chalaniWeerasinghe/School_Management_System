using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class UserSubject
    {
            public string UserId { get; set; } 
            public ApplicationUser User { get; set; } 
            public int SubjectId { get; set; } 
            public Subject Subject { get; set; } 

    }
}
