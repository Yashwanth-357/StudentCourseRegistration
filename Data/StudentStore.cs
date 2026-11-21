using StudentCourseRegistration.Models;

namespace StudentCourseRegistration.Data
{
    /// <summary>
    /// In-memory "database": one static List&lt;Student&gt; shared by every request.
    /// Data is lost when the application restarts (no SQL / Entity Framework needed).
    ///
    /// A web server handles many requests at the same time, so every access to the
    /// list is wrapped in lock(...) to stop two requests changing it simultaneously.
    /// </summary>
    public static class StudentStore
    {
        private static readonly object _lock = new();

        // A few sample records so the list page is not empty on first run.
        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Aarav Sharma",  Email = "aarav.sharma@example.com",  Course = "B.Tech CSE",        Semester = 5, RegisteredOn = DateTime.Now.AddDays(-12) },
            new Student { Id = 2, Name = "Priya Nair",    Email = "priya.nair@example.com",    Course = "B.Tech IT",         Semester = 3, RegisteredOn = DateTime.Now.AddDays(-9) },
            new Student { Id = 3, Name = "Rohan Mehta",   Email = "rohan.mehta@example.com",   Course = "B.Tech ECE",        Semester = 7, RegisteredOn = DateTime.Now.AddDays(-6) },
            new Student { Id = 4, Name = "Sneha Reddy",   Email = "sneha.reddy@example.com",   Course = "B.Tech CSE",        Semester = 1, RegisteredOn = DateTime.Now.AddDays(-3) },
            new Student { Id = 5, Name = "Karthik Iyer",  Email = "karthik.iyer@example.com",  Course = "B.Tech Mechanical", Semester = 4, RegisteredOn = DateTime.Now.AddDays(-1) }
        };

        // Next free Id (declared after _students so the list already exists).
        private static int _nextId = _students.Count + 1;

        /// <summary>Returns a copy of all students (so callers cannot modify the real list).</summary>
        public static List<Student> GetAll()
        {
            lock (_lock)
            {
                return _students.ToList();
            }
        }

        /// <summary>Finds one student by Id, or null when no such student exists.</summary>
        public static Student? GetById(int id)
        {
            lock (_lock)
            {
                return _students.FirstOrDefault(s => s.Id == id);
            }
        }

        /// <summary>True if a student with this email is already registered (case-insensitive).</summary>
        public static bool EmailExists(string email)
        {
            lock (_lock)
            {
                return _students.Any(s => s.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>Gives the student a new Id and adds it to the list.</summary>
        public static void Add(Student student)
        {
            lock (_lock)
            {
                student.Id = _nextId++;
                student.RegisteredOn = DateTime.Now;
                _students.Add(student);
            }
        }
    }
}
