using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistration.Data;
using StudentCourseRegistration.Models;

namespace StudentCourseRegistration.Controllers
{
    public class HomeController : Controller
    {
        // Dashboard: total students, per-course counts and the latest registrations.
        public IActionResult Index()
        {
            var students = StudentStore.GetAll();

            // One entry per course (even if it has 0 students).
            var perCourse = CourseCatalog.Courses.ToDictionary(
                course => course,
                course => students.Count(s => s.Course == course));

            var viewModel = new HomeViewModel
            {
                TotalStudents = students.Count,
                TotalCourses = CourseCatalog.Courses.Count,
                StudentsPerCourse = perCourse,
                MostPopularCourse = students.Count == 0
                    ? "-"
                    : perCourse.OrderByDescending(p => p.Value).First().Key,
                RecentStudents = students.OrderByDescending(s => s.RegisteredOn).Take(5).ToList()
            };

            return View(viewModel);
        }

        // Shared error page. For status-code errors the "code" comes from Program.cs
        // (UseStatusCodePagesWithReExecute), e.g. /Home/Error?code=404
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code)
        {
            var viewModel = new ErrorViewModel
            {
                StatusCode = code ?? 500,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
