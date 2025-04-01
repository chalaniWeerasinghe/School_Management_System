using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using AuthSystem.Data;
using AuthSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

public class StudentsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AuthDbContext _context;

    public StudentsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _userManager.GetUsersInRoleAsync("Student");

        return View(students);
    }

    public IActionResult Create()
    {
        return View();
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }


    [HttpPost]
    public async Task<IActionResult> Create(string email, string password, string firstName, string lastName, DateTime birthDate, string studentNumber)
    {
        if (ModelState.IsValid)
        {
            var student = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                StudentNumber = studentNumber
            };

            var result = await _userManager.CreateAsync(student, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student, "Student");

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View();
    }
    public async Task<IActionResult> Edit(string id)
    {
        var student = await _userManager.FindByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser student)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(student.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = student.FirstName;
            user.LastName = student.LastName;
            user.BirthDate = student.BirthDate;
            user.StudentNumber = student.StudentNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(student);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var student = await _userManager.FindByIdAsync(id);
        if (student != null)
        {
            var result = await _userManager.DeleteAsync(student);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AssignSubjects()
    {
        var users = await _userManager.Users.ToListAsync();
        var subjects = await _context.Subjects.ToListAsync();

        var viewModel = new AssignSubjectsViewModel
        {
            Users = users,
            Subjects = subjects
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AssignSubjects(string userId, List<int> subjectIds)
    {
        if (string.IsNullOrEmpty(userId) || subjectIds == null || !subjectIds.Any())
        {
            ModelState.AddModelError(string.Empty, "Please select a user and at least one subject.");
            return RedirectToAction("AssignSubjects");
        }

        foreach (var subjectId in subjectIds)
        {
            var userSubject = new UserSubject
            {
                UserId = userId,
                SubjectId = subjectId
            };
            _context.UserSubjects.Add(userSubject);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }



}

