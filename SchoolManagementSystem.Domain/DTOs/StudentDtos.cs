using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.DTOs
{
    public class CreateStudentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]

        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid class")]
        public int ClassId { get; set; }
    }
    public class UpdateStudentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid class")]
        public int ClassId { get; set; }
    }
    public class StudentSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public string ClassName { get; set; }
    }

}
