using SchoolManagementSystem.Business.Interfaces;
using SchoolManagementSystem.DataAccess.Interfaces;
using SchoolManagementSystem.Domain.DTOs;
using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToDto);
        }

       

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course == null ? null : MapToDto(course);
        }

        public async Task<CourseWithStudentsDto?> GetCourseWithStudentsAsync(int id)
        {
            var course = await _courseRepository.GetByIdWithDetailsAsync(id);
            return course == null ? null : MapToWithStudentsDto(course);
        }

        public async Task<IEnumerable<CourseSummaryDto>> GetCoursesByTeacherAsync(int teacherId)
        {
            var courses = await _courseRepository.GetByTeacherIdAsync(teacherId);
            return courses.Select(MapToSummaryDto);
        }

        public async Task<IEnumerable<CourseSummaryDto>> GetCoursesByClassAsync(int classId)
        {
            var courses = await _courseRepository.GetByClassIdAsync(classId);
            return courses.Select(MapToSummaryDto);
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            // Validate title uniqueness
            if (await _courseRepository.ExistsByTitleAsync(createCourseDto.Title))
            {
                throw new InvalidOperationException($"Course with title '{createCourseDto.Title}' already exists.");
            }

            // Validate teacher doesn't already teach another course in the same class
            if (await _courseRepository.TeacherHasCourseInClassAsync(createCourseDto.TeacherId, createCourseDto.ClassId))
            {
                throw new InvalidOperationException("The selected teacher already teaches another course in this class.");
            }

            var course = new Course
            {
                Title = createCourseDto.Title.Trim(),
                Credits = createCourseDto.Credits,
                TeacherId = createCourseDto.TeacherId,
                ClassId = createCourseDto.ClassId
            };

            var createdCourse = await _courseRepository.AddAsync(course);
            var courseWithDetails = await _courseRepository.GetByIdAsync(createdCourse.Id);
            return MapToDto(courseWithDetails!);
        }

        public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateCourseDto)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
                return null;

            // Validate title uniqueness (excluding current course)
            if (await _courseRepository.ExistsByTitleAsync(updateCourseDto.Title, id))
            {
                throw new InvalidOperationException($"Another course with title '{updateCourseDto.Title}' already exists.");
            }

            // Validate teacher doesn't already teach another course in the same class (excluding current course)
            if (await _courseRepository.TeacherHasCourseInClassAsync(updateCourseDto.TeacherId, updateCourseDto.ClassId, id))
            {
                throw new InvalidOperationException("The selected teacher already teaches another course in this class.");
            }

            existingCourse.Title = updateCourseDto.Title.Trim();
            existingCourse.Credits = updateCourseDto.Credits;
            existingCourse.TeacherId = updateCourseDto.TeacherId;
            existingCourse.ClassId = updateCourseDto.ClassId;

            var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
            var courseWithDetails = await _courseRepository.GetByIdAsync(updatedCourse.Id);
            return MapToDto(courseWithDetails!);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _courseRepository.DeleteAsync(id);
        }

       

        public async Task<bool> CourseExistsAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course != null;
        }

        public async Task<bool> TitleExistsAsync(string title)
        {
            return await _courseRepository.ExistsByTitleAsync(title);
        }

        public async Task<bool> TitleExistsAsync(string title, int excludeId)
        {
            return await _courseRepository.ExistsByTitleAsync(title, excludeId);
        }

      

        public async Task<(IEnumerable<CourseDto> Courses, int TotalCount)> GetPagedCoursesAsync(int pageNumber, int pageSize)
        {
            var (courses, totalCount) = await _courseRepository.GetPagedAsync(pageNumber, pageSize);
            var courseDtos = courses.Select(MapToDto);
            return (courseDtos, totalCount);
        }

        public async Task<int> GetCoursesCountAsync()
        {
            return await _courseRepository.CountAsync();
        }

        private static CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                
                TeacherId = course.TeacherId,
                TeacherName = course.Teacher.FullName,
                ClassId = course.ClassId,
                ClassName = course.Class?.Name ?? "Unknown",
            };
        }

        private static CourseSummaryDto MapToSummaryDto(Course course)
        {
            return new CourseSummaryDto
            {
                Id = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                TeacherName = course.Teacher.FullName,
                ClassName = course.Class?.Name ?? "Unknown",

            };
        }

        private static CourseWithStudentsDto MapToWithStudentsDto(Course course)
        {
            return new CourseWithStudentsDto
            {
                Id = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                TeacherName = course.Teacher.FullName,
                ClassName = course.Class?.Name ?? "Unknown",
                EnrolledStudents = course.Students?.Select(sc => new StudentSummaryDto
                {
                    Id = sc.Student.Id,
                    FullName = sc.Student.FullName,
                    Email = sc.Student.Email,
                    ClassId=sc.Student.ClassId,
                    ClassName = sc.Student.Class?.Name ?? "Unknown",
                }).ToList() ?? new List<StudentSummaryDto>()
            };
        }

        private static StudentSummaryDto MapStudentToSummaryDto(Student student)
        {
            return new StudentSummaryDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                ClassName = student.Class?.Name ?? "Unknown",
            };
        }
    }
}
