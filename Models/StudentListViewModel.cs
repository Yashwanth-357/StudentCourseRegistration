namespace StudentCourseRegistration.Models
{
    /// <summary>
    /// Data sent to Student/Index.cshtml: the (already filtered) students
    /// plus the current search / filter values so the form can remember them.
    /// </summary>
    public class StudentListViewModel
    {
        public List<Student> Students { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SelectedCourse { get; set; }
        public int TotalCount { get; set; }          // all registered students (before filtering)
        public bool IsFiltered => !string.IsNullOrWhiteSpace(SearchTerm) || !string.IsNullOrWhiteSpace(SelectedCourse);
    }
}
