namespace StudentCourseRegistration.Models
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; } = 500;
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Title => StatusCode switch
        {
            404 => "Page not found",
            403 => "Access denied",
            _ => "Something went wrong"
        };

        public string Message => StatusCode switch
        {
            404 => "The page you are looking for does not exist or has been moved.",
            403 => "You do not have permission to open this page.",
            _ => "An unexpected error occurred while processing your request. Please try again."
        };
    }
}
