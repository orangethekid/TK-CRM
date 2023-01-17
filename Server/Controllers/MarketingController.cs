using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class MarketingController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public MarketingController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( string type )
        {
            try
            {
                switch (type.ToLower())
                {
                    case "facebook":
                        var facebook = await _context.FabookAds.OrderByDescending( fb => fb.Id ).ToListAsync();
                        _logger.LogInfo( $"Returned GetAll facebook Marketing from database." );
                        return Ok( facebook );
                    case "line":
                        var line = await _context.LineAdsPlatforms.OrderByDescending( l => l.Id ).ToListAsync();
                        _logger.LogInfo( $"Returned GetAll line Marketing from database." );
                        return Ok( line );
                    case "google":
                        var google = await _context.GoogleShops.OrderByDescending( ggs => ggs.Id ).ToListAsync();
                        _logger.LogInfo( $"Returned GetAll google Marketing from database." );
                        return Ok( google );
                    default:
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAll Marketing action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMarketingFacebook( int id )
        {
            try
            {
                FacebookAds fb = await _context.FabookAds.FirstOrDefaultAsync( f => f.OrderID == id );
                if (fb == null)
                {
                    _logger.LogInfo( $"Marketing Facebook with id: {id}, hasn't been found in db." );
                    return Ok( new FacebookAds() );
                }
                else
                {
                    _logger.LogInfo( $"Returned Marketing Facebook with id: {id}" );
                    return Ok( fb );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetMarketingFacebook action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMarketingFacebook( FacebookAds fb )
        {
            try
            {
                if (fb == null)
                {
                    _logger.LogError( "FacebookAds object sent from client is null." );
                    return BadRequest( "FacebookAds object is null" );
                }

                _context.Add( fb );
                await _context.SaveChangesAsync();
                return Ok( fb );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateMarketingFacebook action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditMarketingFacebook( FacebookAds fb )
        {
            try
            {
                if (fb == null)
                {
                    _logger.LogError( "FacebookAds object sent from client is null." );
                    return BadRequest( "FacebookAds object is null" );
                }

                _context.Entry( fb ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok( fb );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditMarketingFacebook action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMarketingFacebook( int id )
        {
            try
            {
                FacebookAds fb = new FacebookAds { Id = id };
                if (fb == null)
                {
                    _logger.LogInfo( $"FacebookAds with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( fb );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside DeleteMarketingFacebook action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMarketingLine( int id )
        {
            try
            {
                LineAdsPlatform lap = await _context.LineAdsPlatforms.FirstOrDefaultAsync( l => l.OrderID == id );
                if (lap == null)
                {
                    _logger.LogInfo( $"Marketing LineAdsPlatform with id: {id}, hasn't been found in db." );
                    return Ok( new LineAdsPlatform() );
                }
                else
                {
                    _logger.LogInfo( $"Returned Marketing LineAdsPlatform with id: {id}" );
                    return Ok( lap );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetMarketingLine action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMarketingLine( LineAdsPlatform line )
        {
            try
            {
                if (line == null)
                {
                    _logger.LogError( "LineAdsPlatform object sent from client is null." );
                    return BadRequest( "LineAdsPlatform object is null" );
                }
                _context.Add( line );
                await _context.SaveChangesAsync();
                return Ok( line );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateMarketingLine action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditMarketingLine( LineAdsPlatform line )
        {
            try
            {
                if (line == null)
                {
                    _logger.LogError( "LineAdsPlatform object sent from client is null." );
                    return BadRequest( "LineAdsPlatform object is null" );
                }
                _context.Entry( line ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok( line );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditMarketingLine action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMarketingLine( int id )
        {
            try
            {
                LineAdsPlatform line = new LineAdsPlatform { Id = id };
                if (line == null)
                {
                    _logger.LogError( $"LineAdsPlatform with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( line );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside DeleteMarketingLine action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMarketingGoogle( int id )
        {
            try
            {
                GoogleShop gshop = await _context.GoogleShops.FirstOrDefaultAsync( g => g.OrderID == id );
                if (gshop == null)
                {
                    _logger.LogInfo( $"Marketing GoogleShop with id: {id}, hasn't been found in db." );
                    return Ok( new GoogleShop() );
                }
                else
                {
                    _logger.LogInfo( $"Returned Marketing GoogleShop with id: {id}" );
                    return Ok( gshop );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetMarketingGoogle action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMarketingGoogle( GoogleShop google )
        {
            try
            {
                if (google == null)
                {
                    _logger.LogError( "GoogleShop object sent from client is null." );
                    return BadRequest( "GoogleShop object is null" );
                }
                _context.Add( google );
                await _context.SaveChangesAsync();
                return Ok( google );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateMarketingGoogle action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditMarketingGoogle( GoogleShop google )
        {
            try
            {
                if (google == null)
                {
                    _logger.LogError( "GoogleShop object sent from client is null." );
                    return BadRequest( "GoogleShop object is null" );
                }

                _context.Entry( google ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok( google );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditMarketingGoogle action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMarketingGoogle( int id )
        {
            try
            {
                GoogleShop google = new GoogleShop { Id = id };
                if (google == null)
                {
                    _logger.LogError( $"GoogleShop with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( google );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside DeleteCustomer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }
    }
}
