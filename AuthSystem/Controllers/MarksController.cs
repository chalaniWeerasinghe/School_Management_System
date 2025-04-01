using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    public class MarksController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarksController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var marks = await _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .Include(m => m.Grade)
                .ToListAsync();
            return View(marks);
        }

        public async Task<IActionResult> Create()
        {
            // Fetch all users in the "Student" role
            var allUsers = await _userManager.Users.ToListAsync();
            var studentUsers = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Student"))
                {
                    studentUsers.Add(user);
                }
            }

            // Assign students to ViewBag
            ViewBag.Students = studentUsers;

            // Fetch subjects
            ViewBag.Subjects = await _context.Subjects.ToListAsync();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> MyMarks()
        {
            // Get the currently logged-in user
            var userId = _userManager.GetUserId(User); // This retrieves the logged-in user's ID

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home"); // Redirect if no user is logged in
            }

            // Fetch the marks for the logged-in user
            var marks = await _context.Marks
                .Include(m => m.Subject)
                .Include(m => m.Grade)
                .Where(m => m.StudentId == userId) // Filter by logged-in user's ID
                .ToListAsync();

            return View(marks);
        }



        [HttpPost]
        public async Task<IActionResult> Create(string studentId, int subjectId, int numericMark)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Ensure the student exists
            var student = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            if (student == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid student.");
                return View();
            }

            // Ensure the subject exists
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId);
            if (subject == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid subject.");
                return View();
            }

            // Calculate the grade
            var gradeLetter = GetPerformanceGrade(numericMark);
            var grade = await _context.Grades.FirstOrDefaultAsync(g => g.GradeName == gradeLetter);

            if (grade == null)
            {
                // If the grade doesn't exist in the database, create a new one
                grade = new Grade { GradeName = gradeLetter };
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
            }

            // Create a new Mark entity (without setting the Id manually)
            var mark = new Mark
            {
                StudentId = studentId,
                SubjectId = subjectId,
                NumericMark = numericMark,
                GradeId = grade.GradeId
            };

            // Add the mark entity to the DbContext
            _context.Marks.Add(mark);
            await _context.SaveChangesAsync();

            // Redirect to the Index page or another view
            return RedirectToAction("Index");
        }




        private string GetPerformanceGrade(int numericMark)
        {
            if (numericMark >= 90) return "A";
            if (numericMark >= 75) return "B";
            if (numericMark >= 50) return "C";
            return "F";
        }
    }

}
