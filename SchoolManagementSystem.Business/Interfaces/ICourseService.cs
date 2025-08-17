using SchoolManagementSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<CourseWithStudentsDto?> GetCourseWithStudentsAsync(int id);
        Task<IEnumerable<CourseSummaryDto>> GetCoursesByTeacherAsync(int teacherId);
        Task<IEnumerable<CourseSummaryDto>> GetCoursesByClassAsync(int classId);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateCourseDto);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> CourseExistsAsync(int id);
        Task<bool> TitleExistsAsync(string title);
        Task<bool> TitleExistsAsync(string title, int excludeId);
        Task<(IEnumerable<CourseDto> Courses, int TotalCount)> GetPagedCoursesAsync(int pageNumber, int pageSize);
        Task<int> GetCoursesCountAsync();   
    }
}
