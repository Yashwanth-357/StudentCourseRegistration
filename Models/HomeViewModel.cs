namespace StudentCourseRegistration.Models
{
    /// <summary>Data shown on the dashboard (Home/Index.cshtml).</summary>
    public class HomeViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public string MostPopularCourse { get; set; } = "-";
        public List<Student> RecentStudents { get; set; } = new();

        // Course name -> number of students registered in it.
        public Dictionary<string, int> StudentsPerCourse { get; set; } = new();
    }
}
