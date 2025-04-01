using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class Student
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
