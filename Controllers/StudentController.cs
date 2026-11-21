using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistration.Data;
using StudentCourseRegistration.Models;

namespace StudentCourseRegistration.Controllers
{
    public class StudentController : Controller
    {
        // GET: /Student?searchTerm=asha&course=B.Tech CSE
        // The two parameters are filled from the query string (model binding).
        public IActionResult Index(string? searchTerm, string? course)
        {
            var allStudents = StudentStore.GetAll();
            IEnumerable<Student> result = allStudents;

            // 1. Search by name OR email (case-insensitive "contains").
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                result = result.Where(s =>
                    s.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filter by the selected course.
            if (!string.IsNullOrWhiteSpace(course))
            {
                result = result.Where(s => s.Course == course);
            }

            var viewModel = new StudentListViewModel
            {
                Students = result.OrderBy(s => s.Id).ToList(),
                SearchTerm = searchTerm,
                SelectedCourse = course,
                TotalCount = allStudents.Count
            };

            return View(viewModel);
        }

        // GET: /Student/Create  -> shows the empty registration form
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Email,Course,Semester")] Student student)
        {
            // Extra business rules that Data Annotations cannot express:
            if (!string.IsNullOrWhiteSpace(student.Email) && StudentStore.EmailExists(student.Email))
            {
                ModelState.AddModelError(nameof(student.Email), "This email address is already registered.");
            }

            if (!string.IsNullOrWhiteSpace(student.Course) && !CourseCatalog.Courses.Contains(student.Course))
            {
                ModelState.AddModelError(nameof(student.Course), "Please choose a course from the list.");
            }

            // If any rule failed, show the same form again with the error messages.
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            student.Name = student.Name.Trim();
            student.Email = student.Email.Trim();
            StudentStore.Add(student);

            // TempData survives exactly one redirect - perfect for a "flash" success message.
            TempData["SuccessMessage"] = $"{student.Name} has been registered successfully.";

            // Post-Redirect-Get pattern: stops a duplicate registration if the user presses F5.
            return RedirectToAction(nameof(Details), new { id = student.Id });
        }

        // GET: /Student/Details/3
        public IActionResult Details(int id)
        {
            var student = StudentStore.GetById(id);

            if (student == null)
            {
                // Invalid / unknown Id: send a real 404 status and show a friendly page.
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound", id);
            }

            return View(student);
        }
    }
}
