using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Helpers;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Setting.Course;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public CourseController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] PaginationDTO pagination )
        {
            try
            {
                var queryable = _context.Courses.OrderByDescending( c => c.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable, pagination.QuantityPerPage );
                var courses = await queryable.Paginate( pagination ).ToListAsync();

                _logger.LogInfo( $"Returned all Get Course from database." );

                return Ok( courses );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Get Course action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = await _context.Courses.OrderByDescending( c => c.Id ).ToListAsync();

                _logger.LogInfo( $"Returned GetAll Courses from database." );

                return Ok( courses );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAll Courses action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByCourseId( int CourseId )
        {
            try
            {
                var course = await _context.Courses.FirstOrDefaultAsync( g => g.Id == CourseId );
                if (course == null)
                {
                    _logger.LogError( $"Course with id: {CourseId}, hasn't been found in db." );
                    return NotFound();
                }

                _logger.LogInfo( $"Returned Course with id: {CourseId}" );

                return Ok( course );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Course GetByCourseId action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse( Course course )
        {
            try
            {
                if (course == null)
                {
                    _logger.LogError( "Course object sent from client is null." );
                    return BadRequest( "Course object is null" );
                }
                _context.Courses.Add( course );
                await _context.SaveChangesAsync();
                return Ok( course.Id );

            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Course CreateCourse action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditCourse( Course course )
        {
            try
            {
                if (course == null)
                {
                    _logger.LogError( "Course object sent from client is null." );
                    return BadRequest( "Course object is null" );
                }
                _context.Entry( course ).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Course EditCourse action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse( int id )
        {
            try
            {
                Course course = new Course { Id = id };
                if (course == null)
                {
                    _logger.LogError( $"Course with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( course );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Course DeleteCourse action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }
    }
}
