using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Shared.Graphics;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class GraphicController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public GraphicController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetByOrderId( int OrderId )
        {
            IList<Graphic> graphics = await _context.Graphics.Where( g => g.OrderId == OrderId ).OrderByDescending( g => g.Id ).ToListAsync();
            if (graphics is null || graphics.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok( graphics );
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetByGraphicId( int GraphicId )
        {
            try
            {
                var graphic = await _context.Graphics.Where( g => g.Id == GraphicId ).OrderByDescending( g => g.Id ).ToListAsync();
                if (graphic == null)
                {
                    _logger.LogError( $"Domain with id: {GraphicId}, hasn't been found in db." );
                    return NotFound();
                }

                _logger.LogInfo( $"Returned Domain with id: {GraphicId}" );

                return Ok( graphic );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Graphic GetByGraphicId action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGraphic( Graphic graphic )
        {
            try
            {
                if (graphic == null)
                {
                    _logger.LogError( "Graphic object sent from client is null." );
                    return BadRequest( "Graphic object is null" );
                }
                _context.Graphics.Add( graphic );
                await _context.SaveChangesAsync();
                return Ok( graphic.Id );

            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Graphic CreateGraphic action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditGraphic( Graphic graphic )
        {
            try
            {
                if (graphic == null)
                {
                    _logger.LogError( "Graphic object sent from client is null." );
                    return BadRequest( "Graphic object is null" );
                }
                _context.Entry( graphic ).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Graphic EditGraphic action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGraphic( int id )
        {
            try
            {
                Graphic graphic = new Graphic { Id = id };
                if (graphic == null)
                {
                    _logger.LogError( $"Domain with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( graphic );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Graphic DeleteGraphic action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }
    }
}
