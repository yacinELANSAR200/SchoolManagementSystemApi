using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.DataAccess.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetByClassIdAsync(int classId);
        Task<Student?> GetByEmailAsync(string email);

        Task<IEnumerable<Student>> GetPagedAsync(int pageNumber, int pageSize);
        Task<bool> EmailExistsAsync(string email,int id);
        Task<bool> EmailExistsAsync(string email);
        Task<Student> AddAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();

        Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId);
    }
}
