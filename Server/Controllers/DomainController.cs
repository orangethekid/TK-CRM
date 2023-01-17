using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public DomainController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOne( int id )
        {
            try
            {
                var domain = await _context.Domains.FirstOrDefaultAsync( d => d.Id == id );
                if (domain == null)
                {
                    _logger.LogError( $"Domain with id: {id}, hasn't been found in db." );
                    return NotFound();
                }

                _logger.LogInfo( $"Returned Domain with id: {id}" );

                return Ok( domain );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Domain GetOne action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDomain( Domain domain )
        {
            try
            {
                if (domain == null)
                {
                    _logger.LogError( "Domain object sent from client is null." );
                    return BadRequest( "Domain object is null" );
                }
                _context.Add( domain );
                await _context.SaveChangesAsync();
                await UpdateUrl( domain );
                return Ok( domain );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateDomain action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditDomain( Domain domain )
        {
            try
            {
                if (domain == null)
                {
                    _logger.LogError( "Domain object sent from client is null." );
                    return BadRequest( "Domain object is null" );
                }

                _context.Entry( domain ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await UpdateUrl( domain );
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditCustomer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDomain( int id )
        {
            try
            {
                var domain = new Domain { Id = id };
                if (domain == null)
                {
                    _logger.LogError( $"Domain with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( domain );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside DeleteDomain action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        private async Task UpdateUrl( Domain domain )
        {
            var web = await _context.WebSites.FirstOrDefaultAsync( w => w.Id == domain.WebSiteId );
            if (web == null)
            {
                _logger.LogError( $"Domain with WebSiteId: {domain.WebSiteId}, hasn't been found in db." );
            }
            else
            {
                web.Url = domain.Name;
                _context.Entry( web ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
