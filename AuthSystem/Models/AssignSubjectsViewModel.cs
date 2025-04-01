using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class AssignSubjectsViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<Subject> Subjects { get; set; }
    }

}

