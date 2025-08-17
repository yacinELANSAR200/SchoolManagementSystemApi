using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.DataAccess.Interfaces;
using SchoolManagementSystem.Domain.DTOs;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.DataAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _context;
        public StudentRepository(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(Student => Student.Class)
                .OrderBy(Student => Student.FullName)
                .ToListAsync();
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IEnumerable<Student>> GetByClassIdAsync(int classId)
        {
            return await _context.Students
                .Include(s => s.Class)
                .Where(S => S.ClassId == classId)
                .ToListAsync();
        }
        //update to coursesummarydto
        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                        .Where(sc => sc.StudentId == studentId)
                        .Include(sc => sc.Course)
                        .Select(sc => sc.Course)
                        .ToListAsync();

        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());

        }
        public async Task<bool> EmailExistsAsync(string email,int id)
        {
            return await _context.Students
                .AnyAsync(s => s.Email.ToLower() == email.ToLower() && s.Id != id);

        }
        public async Task<Student> AddAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }
        public async Task<Student> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> CountAsync()
        {
            return await _context.Students.CountAsync();
        }
        public async Task<IEnumerable<Student>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _context.Students
                   .Include(s=> s.Class)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Students
                .AnyAsync(s => s.Email.ToLower() == email.ToLower());
        }
    }
}
