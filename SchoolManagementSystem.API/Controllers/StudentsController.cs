using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Business.Interfaces;
using SchoolManagementSystem.Domain.DTOs;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentService _studentService;
        public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentSummaryDto>>> GetAllStudents()
        {
            try
            {
                var students=await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all students");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentSummaryDto>> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                    return NotFound($"Student with ID {id} not found");

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting student with ID {StudentId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<StudentSummaryDto>>> GetStudentsByClass(int classId)
        {
            try
            {
                var students = await _studentService.GetStudentsByClassIdAsync(classId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting students for class {ClassId}", classId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("paged")]
        public async Task<ActionResult<object>> GetPagedStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var students = await _studentService.GetPagedStudentsAsync(pageNumber, pageSize);

                var response = new
                {
                    Students = students,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                   
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged students");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentSummaryDto>> CreateStudent(CreateStudentDto createStudentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = await _studentService.CreateStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating student");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentSummaryDto>> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var student = await _studentService.UpdateStudentAsync(id, updateStudentDto);
                if (student == null)
                    return NotFound($"Student with ID {id} not found");

                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating student with ID {StudentId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetStudentsCount()
        {
            try
            {
                var count = await _studentService.GetStudentsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting students count");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email, [FromQuery] int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest("Email cannot be empty");

                bool exists = excludeId.HasValue
                    ? await _studentService.EmailExistsAsync(email, excludeId.Value)
                    : await _studentService.EmailExistsAsync(email);

                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking email existence");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("courses/{id}")]
        public async Task<IActionResult> GetCoursesByStudentIdAsync(int studentId)
        {
            try
            {
                var courses = await _studentService.GetCoursesByStudentIdAsync(studentId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting student with ID {studentId}", studentId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);
                if (!result)
                    return NotFound($"Student with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting student with ID {StudentId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
