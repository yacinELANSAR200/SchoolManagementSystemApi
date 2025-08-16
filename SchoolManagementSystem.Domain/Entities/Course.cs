using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Range(1, 10)]
        public int Credits { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey(nameof(Class))]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public ICollection<StudentCourse> Students { get; set; } = new List<StudentCourse>();
    }
}
