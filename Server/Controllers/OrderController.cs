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
using TakraonlineCRM.Shared.Graphics;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private ILoggerManager _logger;

        public OrderController( ApplicationDBContext context, ILoggerManager logger )
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] PaginationDTO pagination )
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var queryable = _context.Orders.OrderByDescending( o => o.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable, pagination.QuantityPerPage );
                var orders = await queryable.Paginate( pagination ).ToListAsync();
                foreach (Order o in orders)
                {
                    orderList.Add( await SetUpOrder( o ) );
                }

                _logger.LogInfo( $"Returned all Get Order from database." );

                return Ok( orderList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Get Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search( [FromQuery] SearchResult searchResult )
        {
            IList<Order> orderList = new List<Order>();
            var queryable = SearchOrder( searchResult, _context.Orders.AsQueryable() );
            await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
            var orders = await queryable.Paginate( searchResult ).ToListAsync();
            foreach (Order o in orders)
            {
                orderList.Add( await SetUpOrder( o ) );
            }
            return Ok( orderList );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var orders = await _context.Orders.OrderByDescending( o => o.Id ).ToListAsync();
                foreach (Order o in orders)
                {
                    orderList.Add( await SetUpOrder( o ) );
                }

                _logger.LogInfo( $"Returned GetAll Order from database." );

                return Ok( orderList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAll Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerId( [FromQuery] PaginationDTO pagination, int customerId )
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var queryable = _context.Orders.OrderByDescending( o => o.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable.Where( o => o.CustomerId == customerId ), pagination.QuantityPerPage );
                var orders = await queryable.Where( o => o.CustomerId == customerId ).Paginate( pagination ).ToListAsync();
                foreach (Order o in orders)
                {
                    orderList.Add( await SetUpOrder( o ) );
                }

                _logger.LogInfo( $"Returned GetByCustomerId Order from database." );

                return Ok( orderList );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetByCustomerId Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByCustomerId( int customerId )
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var orders = await _context.Orders.Where( o => o.CustomerId == customerId ).OrderByDescending( o => o.Id ).ToListAsync();
                if (orders == null)
                {
                    _logger.LogInfo( $"Order with CustomerId: {customerId}, hasn't been found in db." );
                    return Ok( orderList );
                }
                else
                {
                    foreach (Order o in orders)
                    {
                        orderList.Add( await SetUpOrder( o ) );
                    }
                    _logger.LogInfo( $"Returned GetAllByCustomerId Order from database." );
                    return Ok( orderList );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllByCustomerId Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByCretorId( [FromQuery] PaginationDTO pagination, string creatorId )
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var queryable = _context.Orders.OrderByDescending( o => o.Id ).AsQueryable();
                await HttpContext.InsertPaginationParameterInResponse( queryable.Where( o => o.CreatorId == creatorId ), pagination.QuantityPerPage );
                var orders = await _context.Orders.Where( o => o.CreatorId == creatorId ).Paginate( pagination ).ToListAsync();
                if (orders == null)
                {
                    _logger.LogInfo( $"Order with CretorId: {creatorId}, hasn't been found in db." );
                    return Ok( orderList );
                }
                else
                {
                    foreach (Order o in orders)
                    {
                        orderList.Add( await SetUpOrder( o ) );
                    }
                    _logger.LogInfo( $"Returned GetByCretorId Order from database." );
                    return Ok( orderList );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetByCretorId Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByCretorId( string creatorId )
        {
            try
            {
                IList<Order> orderList = new List<Order>();
                var orders = await _context.Orders.Where( o => o.CreatorId == creatorId ).OrderByDescending( o => o.Id ).ToListAsync();
                if (orders == null)
                {
                    _logger.LogInfo( $"Order with CretorId: {creatorId}, hasn't been found in db." );
                    return Ok( orderList );
                }
                else
                {
                    foreach (Order o in orders)
                    {
                        orderList.Add( await SetUpOrder( o ) );
                    }
                    _logger.LogInfo( $"Returned GetAllBySaleId Customer from database." );
                    return Ok( orderList );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetAllByCretorId Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBySaleId( [FromQuery] SearchResult searchResult, string saleId )
        {
            IList<Order> orderList = new List<Order>();
            var customerID = _context.Customers.Where( c => c.SaleId == saleId ).Select( c => c.Id ).ToArray();
            var saleOrders = _context.Orders.Where( o => customerID.Contains( o.CustomerId ) ).AsQueryable();
            var queryable = SearchOrder( searchResult, saleOrders );
            await HttpContext.InsertPaginationParameterInResponse( queryable, searchResult.QuantityPerPage );
            var orders = await queryable.Paginate( searchResult ).ToListAsync();
            foreach (Order o in orders)
            {
                orderList.Add( await SetUpOrder( o ) );
            }
            return Ok( orderList );
        }

        [HttpGet]
        public async Task<IActionResult> GetOne( int id )
        {
            try
            {
                Order order = new Order();
                var o = await _context.Orders.FirstOrDefaultAsync( o => o.Id == id );
                if (o == null)
                {
                    _logger.LogInfo( $"Order with id: {id}, hasn't been found in db." );
                    return Ok( o );
                }
                else
                {
                    order = await SetUpOrder( o );
                    _logger.LogInfo( $"Returned Order with id: {id}" );
                    return Ok( order );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetOne Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxTakraOrderID()
        {
            try
            {
                Order order = new Order();
                var maxValue = (await _context.Orders.MaxAsync( o => (int?) Convert.ToInt32( o.TakraOrderId ) )) ?? 9999;

                var result = await _context.Orders.FirstOrDefaultAsync( o => o.TakraOrderId == maxValue.ToString() );
                if (result == null)
                {
                    _logger.LogInfo( $"Order with Max Value TakraOrderId, hasn't been found in db." );
                    return Ok( order );
                }
                else
                {
                    order = await SetUpOrder( result );
                    _logger.LogInfo( $"Returned Order with Max Value TakraOrderId" );
                    return Ok( order );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetMaxTakraOrderID Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder( Order order )
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError( "Order object sent from client is null." );
                    return BadRequest( "Order object is null" );
                }

                _context.Add( order );
                await _context.SaveChangesAsync();

                return Ok( order.Id );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside CreateOrder Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditOrder( Order order )
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError( "Order object sent from client is null." );
                    return BadRequest( "Order object is null" );
                }

                _context.Entry( order ).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _context.Entry( order.Financial ).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                switch (order.OrderType.ToLower())
                {
                    case "website":
                        _context.Entry( order.Website ).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        break;
                    case "marketing":
                        _context.Entry( order.Marketing ).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        break;
                    case "graphic":
                        _context.Entry( order.Graphic ).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        break;
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside EditOrder action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete( int id )
        {
            try
            {
                var order = new Order { Id = id };
                if (order == null)
                {
                    _logger.LogError( $"Order with id: {id}, hasn't been found in db." );
                    return NotFound();
                }
                _context.Remove( order );
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside Delete Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWebSiteByOrderID( int orderId )
        {
            try
            {
                WebSite web = new WebSite();
                web = await _context.WebSites.FirstOrDefaultAsync( w => w.OrderId == orderId );
                if (web == null)
                {
                    _logger.LogInfo( $"WebSite by OrderId: {orderId}, hasn't been found in db." );
                    web = new WebSite();
                }
                else
                    _logger.LogInfo( $"Returned WebSite by OrderId: {orderId}" );
                return Ok( web );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetWebSiteByOrderID Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDomainByOrderID( int orderId )
        {
            try
            {
                Domain domain = new Domain();
                domain = await _context.Domains.FirstOrDefaultAsync( d => d.OrderId == orderId );
                if (domain == null)
                {
                    _logger.LogInfo( $"Domain by OrderId: {orderId}, hasn't been found in db." );
                    domain = new Domain();
                }
                else
                    _logger.LogInfo( $"Returned Domain by OrderId: {orderId}" );

                return Ok( domain );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetDomainByOrderID Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGraphicByOrderID( int orderId )
        {
            try
            {
                IList<Graphic> graphics = new List<Graphic>();
                graphics = await _context.Graphics.Where( g => g.OrderId == orderId ).OrderByDescending( g => g.Id ).ToListAsync();
                if (graphics == null)
                {
                    _logger.LogInfo( $"Graphic Design by OrderId: {orderId}, hasn't been found in db" );
                    graphics = new List<Graphic>();
                }
                else
                    _logger.LogInfo( $"Returned Graphic Design by OrderId: {orderId}" );

                return Ok( graphics );
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Something went wrong inside GetDomainByOrderID Order action: {ex.Message}" );
                return StatusCode( 500, "Internal server error" );
            }
        }

        private async Task<Order> SetUpOrder( Order order )
        {
            Order _order = new Order();
            _order = order;
            _order.Financial = await _context.OrderFinancials.FirstOrDefaultAsync( f => f.OrderId == order.Id );
            switch (_order.OrderType)
            {
                case "website":
                    _order.Website = await _context.OrderWebSites.FirstOrDefaultAsync( w => w.OrderId == order.Id );
                    break;
                case "marketing":
                    _order.Marketing = await _context.OrderMarketings.FirstOrDefaultAsync( m => m.OrderId == order.Id );
                    break;
                case "graphic":
                    _order.Graphic = await _context.OrderGraphics.FirstOrDefaultAsync( g => g.OrderId == order.Id );
                    break;
                case "course":
                    _order.Course = await _context.OrderCourses.FirstOrDefaultAsync( c => c.OrderId == order.Id );
                    break;
            }

            return _order;
        }

        private IQueryable<Order> SearchOrder( SearchResult searchResult, IQueryable<Order> orders )
        {
            var query = orders;
            string SortOrder = searchResult.SortOrder;
            string Filter = searchResult.CurrentFilter;
            string SearchString = searchResult.SearchString;
            if (!string.IsNullOrEmpty( Filter ))
            {
                if (Filter.Contains( "date" ))
                {
                    SearchString = "date";
                }
            }

            //Filter
            if (!string.IsNullOrEmpty( Filter ) && !string.IsNullOrEmpty( SearchString ))
            {
                switch (Filter)
                {
                    case "takraid":
                        query = orders.Where( o => o.TakraOrderId.Contains( SearchString ) );
                        break;
                    case "type":
                        query = orders.Where( o => o.OrderType == SearchString );
                        break;
                    case "priceUp":
                        query = orders.Where( o => o.Financial.Price >= Double.Parse( SearchString ) );
                        break;
                    case "priceDown":
                        query = orders.Where( o => o.Financial.Price <= Double.Parse( SearchString ) );
                        break;
                    case "date":
                        query = orders.Where( o => o.OrderDate >= searchResult.startDate && o.OrderDate <= searchResult.endDate );
                        break;
                    case "orderstatus":
                        query = orders.Where( o => o.OrderStatus == SearchString );
                        break;
                }
            }

            //Sort
            if (!string.IsNullOrEmpty( SortOrder ))
            {
                switch (SortOrder.ToLower())
                {
                    case "id":
                        query = query.OrderBy( o => o.Id );
                        break;
                    case "takraid":
                        query = query.OrderBy( o => o.TakraOrderId );
                        break;
                    case "createdate":
                        query = query.OrderBy( o => o.CreateDate );
                        break;
                    case "price":
                        query = query.OrderBy( o => o.Financial.Price );
                        break;
                    case "descending":
                        query = query.OrderByDescending( o => o.CreateDate );
                        break;
                    default:
                        query = query.OrderByDescending( o => o.CreateDate );
                        break;
                }
            }

            return query;
        }

    }
}

