using SchoolManagementSystem.Domain.DTOs;
using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);

        Task<IEnumerable<StudentDto>> GetStudentsByClassIdAsync(int classId);

        Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId);
        Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto);

        Task<bool> DeleteStudentAsync(int id);
        Task<bool> EmailExistsAsync(string email);

        Task<bool> EmailExistsAsync(string email,int id);
        Task<IEnumerable<StudentDto>> GetPagedStudentsAsync(int pageNumber, int pageSize);
        Task<int> GetStudentsCountAsync();

    }
}
