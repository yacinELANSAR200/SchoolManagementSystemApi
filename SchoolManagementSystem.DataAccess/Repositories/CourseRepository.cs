using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.DataAccess.Interfaces;
using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.DataAccess.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolDbContext _context;

        public CourseRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .Include(c=>c.Students)
                .ThenInclude(c=>c.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .Where(c => c.TeacherId == teacherId)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetByClassIdAsync(int classId)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .Where(c => c.ClassId == classId)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        

        public async Task<Course?> GetByTitleAsync(string title)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)
                .FirstOrDefaultAsync(c => c.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Courses
                .AnyAsync(c => c.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> ExistsByTitleAsync(string title, int excludeId)
        {
            return await _context.Courses
                .AnyAsync(c => c.Title.ToLower() == title.ToLower() && c.Id != excludeId);
        }

        public async Task<bool> TeacherHasCourseInClassAsync(int teacherId, int classId, int? excludeCourseId = null)
        {
            var query = _context.Courses
                .Where(c => c.TeacherId == teacherId && c.ClassId == classId);

            if (excludeCourseId.HasValue)
                query = query.Where(c => c.Id != excludeCourseId.Value);

            return await query.AnyAsync();
        }

        public async Task<Course> AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;

            //var studentCourses = await _context.StudentCourses
            //    .Where(sc => sc.CourseId == id)
            //    .ToListAsync();

            //_context.StudentCourses.RemoveRange(studentCourses);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        

        public async Task<int> CountAsync()
        {
            return await _context.Courses.CountAsync();
        }

       
        
        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Courses.CountAsync();

            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Class)

                .OrderBy(c => c.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalCount);
        }

        
       

       
        
    }
}
