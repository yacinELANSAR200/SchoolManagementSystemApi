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
        Task<IEnumerable<StudentSummaryDto>> GetAllStudentsAsync();
        Task<StudentSummaryDto?> GetStudentByIdAsync(int id);

        Task<IEnumerable<StudentSummaryDto>> GetStudentsByClassIdAsync(int classId);

        Task<IEnumerable<CourseSummaryDto>> GetCoursesByStudentIdAsync(int studentId);
        Task<StudentSummaryDto> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentSummaryDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto);

        Task<bool> DeleteStudentAsync(int id);
        Task<bool> EmailExistsAsync(string email);

        Task<bool> EmailExistsAsync(string email,int id);
        Task<IEnumerable<StudentSummaryDto>> GetPagedStudentsAsync(int pageNumber, int pageSize);
        Task<int> GetStudentsCountAsync();

    }
}
