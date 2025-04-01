
using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class Mark
    { // Define the primary key for the Mark entity
            public int Id { get; set; }  // You can also use other unique identifiers

            // Foreign Key references
            public string StudentId { get; set; }
            public ApplicationUser Student { get; set; }

            public int SubjectId { get; set; }
            public Subject Subject { get; set; }

            public int NumericMark { get; set; }
            public int GradeId { get; set; }
            public Grade Grade { get; set; }
        
    }


    }
