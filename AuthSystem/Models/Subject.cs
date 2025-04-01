namespace AuthSystem.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }
        public ICollection<UserSubject> UserSubjects { get; set; }
        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
