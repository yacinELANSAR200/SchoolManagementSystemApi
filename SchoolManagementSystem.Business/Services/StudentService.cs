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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<IEnumerable<StudentSummaryDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToDto);
        }
        public async Task<StudentSummaryDto?> GetStudentByIdAsync(int id)
        {
            var student=await _studentRepository.GetByIdAsync(id);
            return student==null ? null : MapToDto(student);
        }

        //todo:replace Course by CourseDto
        public async Task<IEnumerable<CourseSummaryDto>> GetCoursesByStudentIdAsync(int id)
        {
            var courses= await _studentRepository.GetCoursesByStudentIdAsync(id);
            return courses.Select(c => new CourseSummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits,
                ClassName = c.Class?.Name ?? "Unknown",
                TeacherName = c.Teacher?.FullName ?? "Unknown"
            });
        }

        public async Task<IEnumerable<StudentSummaryDto>> GetStudentsByClassIdAsync(int classId)
        {
            var students = await _studentRepository.GetByClassIdAsync(classId);
            return students.Select(MapToDto);
        }

        public async Task<StudentSummaryDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            if (await _studentRepository.EmailExistsAsync(createStudentDto.Email))
            {
                throw new InvalidOperationException($"Student with email {createStudentDto.Email} already exists.");
            }
            var age = DateTime.Now.Year - createStudentDto.DateOfBirth.Year;
            if (createStudentDto.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            if(age<5)
                throw new InvalidOperationException("Student must be at least 5 years old.");

            var student = new Student
            {
                FullName = createStudentDto.FullName,
                DateOfBirth = createStudentDto.DateOfBirth,
                Email = createStudentDto.Email,
                PhoneNumber = createStudentDto.PhoneNumber,
                ClassId = createStudentDto.ClassId
            };
            var createdStudent=await _studentRepository.AddAsync(student);
            var studentWithClasses=await _studentRepository.GetByIdAsync(createdStudent.Id);
            return MapToDto(studentWithClasses!);

        }
        public async Task<StudentSummaryDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto)
        {
            var existingStudent=await _studentRepository.GetByIdAsync(id);

            if (existingStudent == null) return null;

            if (await _studentRepository.EmailExistsAsync(updateStudentDto.Email, id))
                throw new InvalidOperationException($"Another student with email {updateStudentDto.Email} already exists.");

            var age = DateTime.Now.Year - updateStudentDto.DateOfBirth.Year;
            if (updateStudentDto.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            if (age < 5)
                throw new InvalidOperationException("Student must be at least 5 years old.");

            existingStudent.FullName = updateStudentDto.FullName.Trim();
            existingStudent.Email = updateStudentDto.Email.Trim();
            existingStudent.PhoneNumber=updateStudentDto.PhoneNumber.Trim();
            existingStudent.DateOfBirth = updateStudentDto.DateOfBirth;
            existingStudent.ClassId=updateStudentDto.ClassId;

            var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
            var studentWithClass = await _studentRepository.GetByIdAsync(updatedStudent.Id);
            return MapToDto(studentWithClass!);
        }
        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _studentRepository.EmailExistsAsync(email);
        }

        public async Task<bool> EmailExistsAsync(string email, int excludeId)
        {
            return await _studentRepository.EmailExistsAsync(email, excludeId);
        }

        public async Task<IEnumerable<StudentSummaryDto>> GetPagedStudentsAsync(int pageNumber, int pageSize)
        {
            var students=await _studentRepository.GetPagedAsync(pageNumber, pageSize);
            return students.Select(MapToDto);
        }
        public async Task<int> GetStudentsCountAsync()
        {
           return await _studentRepository.CountAsync();
        }

        private static StudentSummaryDto MapToDto(Student student)
        {
            return new StudentSummaryDto()
            {
                Id = student.Id,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                ClassId=student.ClassId,
                ClassName = student.Class?.Name ?? "Unknown"
            };
        }

        
    }
}
