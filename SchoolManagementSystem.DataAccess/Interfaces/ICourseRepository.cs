using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.DataAccess.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId);
        Task<IEnumerable<Course>> GetByClassIdAsync(int classId);
        Task<Course?> GetByTitleAsync(string title);
        Task<bool> ExistsByTitleAsync(string title);
        Task<bool> ExistsByTitleAsync(string title, int excludeId);
        Task<bool> TeacherHasCourseInClassAsync(int teacherId, int classId, int? excludeCourseId = null);
        Task<Course> AddAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }
}
