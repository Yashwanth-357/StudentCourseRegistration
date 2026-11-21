namespace StudentCourseRegistration.Models
{
    /// <summary>
    /// The list of courses a student can register for.
    /// Kept in one place so the registration form, the filter dropdown
    /// and the server-side check all use exactly the same list.
    /// </summary>
    public static class CourseCatalog
    {
        public static readonly IReadOnlyList<string> Courses = new List<string>
        {
            "B.Tech CSE",
            "B.Tech IT",
            "B.Tech ECE",
            "B.Tech EEE",
            "B.Tech Mechanical",
            "B.Tech Civil"
        };
    }
}
