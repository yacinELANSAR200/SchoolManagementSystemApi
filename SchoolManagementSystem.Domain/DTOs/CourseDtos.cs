using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
    }

    public class CreateCourseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        public int Credits { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid class")]
        public int ClassId { get; set; }
    }

    public class UpdateCourseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        public int Credits { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid class")]
        public int ClassId { get; set; }

    }

    public class CourseSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        
    }

    public class CourseWithStudentsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public List<StudentSummaryDto> EnrolledStudents { get; set; } = new();
    }

}
