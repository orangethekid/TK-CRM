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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ILoggerManager _logger;

        public CustomerController( ApplicationDBContext context, UserManager<ApplicationUser> userManager, ILoggerManager logger )
        {
            this._context = context;
            this._userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] PaginationDTO pagination )
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var queryable = _context.Customers.OrderByDescending( c => c.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable, pagination.QuantityPerPage );
                var customers = await queryable.Paginate( pagination ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned all Get Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Get Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search( [FromQuery] SearchResult searchResult )
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var queryable = SearchCustomer( searchResult, _context.Customers.AsQueryable() );
                await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
                var customers = await queryable.Paginate( searchResult ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned all Search Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Search Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var customers = await _context.Customers.OrderByDescending( c => c.Id ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned GetAll Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAll Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByCreatorId( string creatorId )
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var customers = await _context.Customers.Where( c => c.CreatorId == creatorId ).OrderByDescending( c => c.Id ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned GetAllByCreatorId Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllByCreatorId Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBySaleId( string saleId )
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var customers = await _context.Customers.Where( c => c.SaleId == saleId ).OrderByDescending( c => c.Id ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned GetAllBySaleId Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllBySaleId Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchBySaleId( [FromQuery] SearchResult searchResult, string saleId )
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                var queryable = SearchCustomer( searchResult, _context.Customers.Where( c => c.SaleId == saleId ).AsQueryable() );
                await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
                var customers = await queryable.Paginate( searchResult ).ToListAsync();
                foreach (Customer cs in customers)
                {
                    customerList.Add( await SetUpCustomer( cs ) );
                }

                _logger.LogInfo( $"Returned GetAllBySaleId Customer from database." );

                return Ok( customerList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllBySaleId Customer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOneById( int id )
        {
            try
            {
                Customer customer = new Customer();
                var cs = await _context.Customers.FirstOrDefaultAsync( a => a.Id == id );
                if (cs == null)
                {
                    _logger.LogError( $"Customer with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                else
                {
                    customer = await SetUpCustomer( cs );

                    _logger.LogInfo( $"Returned Customer with id: {id}" );

                    return Ok( customer );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetOneById action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer( Customer Customer )
        {
            try
            {
                if (Customer == null)
                {
                    _logger.LogError( "Customer object sent from client is null." );
                    return BadRequest( "Customer object is null" );
                }

                if (string.IsNullOrEmpty( Customer.SaleId ))
                {
                    Customer.SaleId = Customer.CreatorId;
                }
                _context.Add( Customer );
                await _context.SaveChangesAsync();
                return Ok( Customer.Id );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateCustomer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditCustomer( Customer Customer )
        {
            try
            {
                if (Customer == null)
                {
                    _logger.LogError( "Customer object sent from client is null." );
                    return BadRequest( "Customer object is null" );
                }

                _context.Entry( Customer ).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditCustomer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer( int id )
        {
            try
            {
                var customer = new Customer { Id = id };
                if (customer == null)
                {
                    _logger.LogError( $"Customer with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( customer );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside DeleteCustomer action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        private IQueryable<Customer> SearchCustomer( SearchResult searchResult, IQueryable<Customer> customers )
        {
            var query = customers;
            string SortOrder = searchResult.SortOrder;
            string Filter = searchResult.CurrentFilter;
            string SearchString = searchResult.SearchString;

            //Filter
            if (!string.IsNullOrEmpty( Filter ) && !string.IsNullOrEmpty( SearchString ))
            {
                switch (Filter)
                {
                    case "id":
                        query = customers.Where( c => c.Id == int.Parse( SearchString ) );
                        break;
                    case "name":
                        query = customers.Where( c => c.FirstName.Contains( SearchString ) );
                        break;
                    case "businsstype":
                        query = customers.Where( c => c.BusinessType.Contains( SearchString ) );
                        break;
                    case "businessname":
                        query = customers.Where( c => c.BusinessName.Contains( SearchString ) );
                        break;
                    case "phone":
                        query = customers.Where( c => c.Phone.Contains( SearchString ) );
                        break;
                    case "email":
                        query = customers.Where( c => c.Email.Contains( SearchString ) );
                        break;
                    case "facebook":
                        query = customers.Where( c => c.Facebok.Contains( SearchString ) );
                        break;
                    case "line":
                        query = customers.Where( c => c.Line.Contains( SearchString ) );
                        break;
                }
            }

            //Sort
            if (!string.IsNullOrEmpty( SortOrder ))
            {
                switch (SortOrder.ToLower())
                {
                    case "id":
                        query = query.OrderBy( c => c.Id );
                        break;
                    case "name":
                        query = query.OrderBy( c => c.FirstName ).ThenBy( c => c.LastName );
                        break;
                    case "phone":
                        query = query.OrderBy( c => c.Phone );
                        break;
                    case "createdate":
                        query = query.OrderBy( c => c.CreateDate );
                        break;
                    case "createorid":
                        query = query.OrderBy( c => c.CreatorId );
                        break;
                    case "saleid":
                        query = query.OrderBy( c => c.SaleId );
                        break;
                    case "descending":
                        query = query.OrderByDescending( c => c.CreateDate );
                        break;
                    default:
                        query = query.OrderByDescending( c => c.CreateDate );
                        break;
                }
            }

            return query;
        }

        private async Task<Customer> SetUpCustomer( Customer customer )
        {
            Customer _customer = new Customer();
            _customer = customer;
            _customer.WebSites = await _context.WebSites.Where( c => c.CustomerId == _customer.Id ).ToListAsync();
            _customer.Orders = await _context.Orders.Where( c => c.CustomerId == _customer.Id ).ToListAsync();
            foreach (Order o in _customer.Orders)
            {
                o.Financial = await _context.OrderFinancials.FirstOrDefaultAsync( f => f.OrderId == o.Id );
                switch (o.OrderType)
                {
                    case "website":
                        o.Website = await _context.OrderWebSites.FirstOrDefaultAsync( w => w.OrderId == o.Id );
                        break;
                    case "marketing":
                        o.Marketing = await _context.OrderMarketings.FirstOrDefaultAsync( m => m.OrderId == o.Id );
                        break;
                    case "graphic":
                        o.Graphic = await _context.OrderGraphics.FirstOrDefaultAsync( g => g.OrderId == o.Id );
                        break;
                    case "course":
                        o.Course = await _context.OrderCourses.FirstOrDefaultAsync( c => c.OrderId == o.Id );
                        break;
                }
            }

            return _customer;
        }
    }
}