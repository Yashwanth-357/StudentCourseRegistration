using System.ComponentModel.DataAnnotations;

namespace StudentCourseRegistration.Models
{
    /// <summary>
    /// The Student model. Data Annotations (the [attributes]) describe the validation rules.
    /// MVC reads them for server-side validation (ModelState) and the Tag Helpers
    /// turn them into data-val-* attributes for client-side validation.
    /// </summary>
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 characters.")]
        [Display(Name = "Full name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a course.")]
        public string Course { get; set; } = string.Empty;

        // int? (nullable) so that an untouched dropdown gives "required" instead of a silent 0.
        [Required(ErrorMessage = "Please select a semester.")]
        [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8.")]
        public int? Semester { get; set; }

        // Set by the application when the student is saved (not typed in the form).
        [Display(Name = "Registered on")]
        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        // ---- Read-only helpers used by the views (no setter, so never model-bound) ----

        /// <summary>Display code such as STU-0004.</summary>
        public string StudentCode => $"STU-{Id:D4}";

        /// <summary>Initials shown inside the round avatar, e.g. "Asha Rao" -> "AR".</summary>
        public string Initials
        {
            get
            {
                // (Name ?? "") because MVC may read this property while Name is still null
                // (e.g. the form was submitted with the name box empty).
                var parts = (Name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return "?";
                if (parts.Length == 1) return parts[0][..1].ToUpper();
                return (parts[0][..1] + parts[^1][..1]).ToUpper();
            }
        }

        /// <summary>0-4, picks one of five avatar colours in site.css.</summary>
        public int AvatarTone => Id % 5;
    }
}
