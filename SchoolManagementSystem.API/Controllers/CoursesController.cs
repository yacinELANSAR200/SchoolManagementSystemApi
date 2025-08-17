using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Business.Interfaces;
using SchoolManagementSystem.Domain.DTOs;

namespace SchoolManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all courses");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                    return NotFound($"Course with ID {id} not found");

                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting course with ID {CourseId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}/with-students")]
        public async Task<ActionResult<CourseWithStudentsDto>> GetCourseWithStudents(int id)
        {
            try
            {
                var course = await _courseService.GetCourseWithStudentsAsync(id);
                if (course == null)
                    return NotFound($"Course with ID {id} not found");

                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting course with students for ID {CourseId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<CourseSummaryDto>>> GetCoursesByTeacher(int teacherId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByTeacherAsync(teacherId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting courses for teacher {TeacherId}", teacherId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<CourseSummaryDto>>> GetCoursesByClass(int classId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByClassAsync(classId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting courses for class {ClassId}", classId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<object>> GetPagedCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var (courses, totalCount) = await _courseService.GetPagedCoursesAsync(pageNumber, pageSize);

                var response = new
                {
                    Courses = courses,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged courses");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto createCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var course = await _courseService.CreateCourseAsync(createCourseDto);
                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating course");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var course = await _courseService.UpdateCourseAsync(id, updateCourseDto);
                if (course == null)
                    return NotFound($"Course with ID {id} not found");

                return Ok(course);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating course with ID {CourseId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await _courseService.DeleteCourseAsync(id);
                if (!result)
                    return NotFound($"Course with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting course with ID {CourseId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCoursesCount()
        {
            try
            {
                var count = await _courseService.GetCoursesCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting courses count");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


        [HttpGet("title-exists")]
        public async Task<ActionResult<bool>> CheckTitleExists([FromQuery] string title, [FromQuery] int? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest("Title cannot be empty");

                bool exists = excludeId.HasValue
                    ? await _courseService.TitleExistsAsync(title, excludeId.Value)
                    : await _courseService.TitleExistsAsync(title);

                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking title existence");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        

        

    }
}

