using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Helpers;
using TakraonlineCRM.Server.Models;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public LogController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] SearchResult searchResult )
        {
            try
            {
                var queryable = SearchLog( searchResult, _context.ActivitiesLogs.AsQueryable() );
                await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
                var logs = await queryable.Paginate( searchResult ).ToListAsync();

                _logger.LogInfo( $"Return all logs." );
                return Ok( logs );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Get Log action: {ex.ToString()}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLog( ActivitiesLog logs )
        {
            try
            {
                if (logs == null)
                {
                    _logger.LogError( "ActivitiesLog Object sent from client is null. " );
                    return BadRequest( "ActivitiesLog Object is null." );
                }

                _context.Add( logs );
                await _context.SaveChangesAsync();
                return Ok( logs );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Create ActivitiesLog action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        private IQueryable<ActivitiesLog> SearchLog( SearchResult searchResult, IQueryable<ActivitiesLog> logs )
        {
            var query = logs;
            string SortOrder = searchResult.SortOrder;
            string Filter = searchResult.CurrentFilter;
            string SearchString = searchResult.SearchString;
            if(Filter == "date")
            {
                SearchString = "date";
            }

            //Filter
            if (!string.IsNullOrEmpty( Filter ) && !string.IsNullOrEmpty( SearchString ))
            {
                switch( Filter )
                {
                    case "userdisplayname":
                        query = logs.Where( l => l.UserDisplayName.Contains( SearchString ) );
                        break;
                    case "userrole":
                        query = logs.Where( l => l.UserRole == SearchString );
                        break;
                    case "date":
                        query = logs.Where( l => l.TimeStamp >= searchResult.startDate && l.TimeStamp <= searchResult.endDate );
                        break;
                }
            }

            //Sort
            if (!string.IsNullOrEmpty( SortOrder ))
            {
                switch (SortOrder)
                {
                    default:
                        query = query.OrderByDescending( o => o.TimeStamp );
                        break;
                }
            }

            return query;
        }

    }
}
