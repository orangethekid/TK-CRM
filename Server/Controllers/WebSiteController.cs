using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Helpers;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class WebSiteController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public WebSiteController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] PaginationDTO pagination )
        {
            try
            {
                List<WebSite> websiteList = new List<WebSite>();
                var queryable = _context.WebSites.OrderByDescending( w => w.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable, pagination.QuantityPerPage );
                var webSites = await queryable.Paginate( pagination ).ToListAsync();
                foreach (WebSite web in webSites)
                {
                    websiteList.Add( await SetUpWebSite( web ) );
                }
                _logger.LogInfo( $"Returned all Get Website from database." );

                return Ok( websiteList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Get Website action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search( [FromQuery] SearchResult searchResult )
        {
            try
            {
                List<WebSite> websiteList = new List<WebSite>();
                var queryable = SearchWebSite( searchResult, _context.WebSites.AsQueryable() );
                await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
                var websites = await queryable.Paginate( searchResult ).ToListAsync();
                foreach (WebSite web in websites)
                {
                    websiteList.Add( await SetUpWebSite( web ) );
                }

                _logger.LogInfo( $"Returned all Search WebSite from database." );

                return Ok( websiteList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Search WebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<WebSite> websiteList = new List<WebSite>();
                var webSites = await _context.WebSites.OrderByDescending( w => w.Id ).ToListAsync();
                foreach (WebSite web in webSites)
                {
                    websiteList.Add( await SetUpWebSite( web ) );
                }

                _logger.LogInfo( $"Returned GetAll WebSite from database." );

                return Ok( websiteList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAll WebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByCustomer( int customerId )
        {
            try
            {
                List<WebSite> websiteList = new List<WebSite>();
                var webSites = await _context.WebSites.Where( w => w.CustomerId == customerId ).OrderByDescending( w => w.Id ).ToListAsync();
                foreach (WebSite web in webSites)
                {
                    websiteList.Add( await SetUpWebSite( web ) );
                }

                _logger.LogInfo( $"Returned GetAllByCustomer WebSite from database." );

                return Ok( websiteList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllByCustomer WebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOne( int id )
        {
            try
            {
                WebSite webSite = new WebSite();
                var web = await _context.WebSites.FirstOrDefaultAsync( w => w.Id == id );
                if (web == null)
                {
                    _logger.LogInfo( $"WebSite with id: {id}, hasn't been found in db." );
                    return Ok( web );
                }
                else
                {
                    webSite = await SetUpWebSite( web );
                    _logger.LogInfo( $"Returned WebSite with id: {id}" );
                    return Ok( webSite );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetOne WebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWebSite( WebSite webSite )
        {
            try
            {
                if (webSite == null)
                {
                    _logger.LogError( "WebSite object sent from client is null." );
                    return BadRequest( "WebSite object is null" );
                }

                _context.Add( webSite );
                await _context.SaveChangesAsync();
                return Ok( webSite.Id );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateWebSite WebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditWebSite( WebSite webSite )
        {
            try
            {
                if (webSite == null)
                {
                    _logger.LogError( "WebSite object sent from client is null." );
                    return BadRequest( "WebSite object is null" );
                }

                _context.Entry( webSite ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok( webSite );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditWebSite action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWebSite( int id )
        {
            try
            {
                var webSite = new WebSite { Id = id };
                if (webSite == null)
                {
                    _logger.LogError( $"WebSite with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( webSite );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Delete Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        private IQueryable<WebSite> SearchWebSite( SearchResult searchResult, IQueryable<WebSite> websites )
        {
            var query = websites;
            string SortOrder = searchResult.SortOrder;
            string Filter = searchResult.CurrentFilter;
            string SearchString = searchResult.SearchString;

            //Filter
            if (!string.IsNullOrEmpty( Filter ) && !string.IsNullOrEmpty( SearchString ))
            {
                switch (Filter.ToLower())
                {
                    case "id":
                        query = websites.Where( w => w.Id == int.Parse( SearchString ) );
                        break;
                    case "name":
                        query = websites.Where( w => w.Name.Contains( SearchString ) );
                        break;
                    case "customerid":
                        query = websites.Where( w => w.CustomerId == int.Parse( SearchString ) );
                        break;
                    case "orderid":
                        query = websites.Where( w => w.OrderId == int.Parse( SearchString ) );
                        break;
                    case "url":
                        query = websites.Where( w => w.Url.Contains( SearchString ) );
                        break;
                    case "version":
                        query = websites.Where( w => w.Version.Contains( SearchString ) );
                        break;
                    case "maximumproduct":
                        query = websites.Where( w => w.MaximumProduct == int.Parse( SearchString ) );
                        break;
                }
            }

            //Sort
            if (!string.IsNullOrEmpty( SortOrder ))
            {
                switch (SortOrder.ToLower())
                {
                    case "id":
                        query = query.OrderBy( w => w.Id );
                        break;
                    case "name":
                        query = query.OrderBy( w => w.Name );
                        break;
                    case "customerid":
                        query = query.OrderBy( w => w.CustomerId );
                        break;
                    case "orderid":
                        query = query.OrderBy( w => w.OrderId );
                        break;
                    case "url":
                        query = query.OrderBy( w => w.Url );
                        break;
                    case "version":
                        query = query.OrderBy( w => w.Version );
                        break;
                    case "descending":
                        query = query.OrderByDescending( w => w.CreateDate );
                        break;
                    default:
                        query = query.OrderByDescending( w => w.CreateDate );
                        break;
                }
            }

            return query;
        }

        private async Task<WebSite> SetUpWebSite( WebSite webSite )
        {
            WebSite _webSite = new WebSite();
            _webSite = webSite;
            _webSite.Domains = await _context.Domains.Where( d => d.WebSiteId == _webSite.Id ).ToListAsync();
            return _webSite;
        }
    }
}
