using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using AuthSystem.Data;
using AuthSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Areas.Identity.Data;

    public class TeachersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _context;

        public TeachersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }



    [HttpPost]
    public async Task<IActionResult> Create(string email, string password, string firstName, string lastName, DateTime birthDate, string employeeNumber)
    {
        if (ModelState.IsValid)
        {
            var teacher = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                EmployeeNumber = employeeNumber
            };

            var result = await _userManager.CreateAsync(teacher, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacher, "Teacher");

                return RedirectToAction("Index", "Teachers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View();
    }

    public IActionResult Students()
    {
        return View("Index", "Students");
    }

    public IActionResult Subjects()
    {
        return View("Index", "Subjects");
    }

    public IActionResult Marks()
    {
        return View("Index", "Marks");
    }

    public async Task<IActionResult> Index()
    {
        var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
        return View(teachers);
    }
}


