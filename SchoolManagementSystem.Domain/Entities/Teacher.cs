using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SubjectSpecialty { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
