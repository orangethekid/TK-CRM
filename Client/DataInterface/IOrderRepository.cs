using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Graphics;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IOrderRepository
    {
        Task<(int totalPageQuantity, IList<Order> orderList)> Get( int page = 1, int quantityPerPage = 10 );
        Task<(int totalPageQuantity, IList<Order> orderList)> Get( SearchResult result );
        Task<IList<Order>> GetAll();
        Task<(int totalPageQuantity, IList<Order> orderList)> GetByCustomerId( int customerId = 1, int page = 1, int quantityPerPage = 10 );
        Task<IList<Order>> GetAllByCustomerId( int customerId );
        Task<(int totalPageQuantity, IList<Order> orderList)> GetByCretorId( string creatorId = "1", int page = 1, int quantityPerPage = 10 );
        Task<IList<Order>> GetAllByCretorId( string creatorId );
        Task<(int totalPageQuantity, IList<Order> orderList)> GetBySaleId( int page, int quantityPerPage, string saleId );
        Task<(int totalPageQuantity, IList<Order> orderList)> GetBySaleId( SearchResult result, string saleId );
        Task<Order> GetOne( int id );
        Task<Order> GetMaxTakraOrderID();
        Task<Order> CreateOrder( Order order, int customerId, string creatorId );
        Task<Order> EditOrder( Order order );
        void Delete( int id );

        Task<WebSite> GetWebSiteByOrderId( int orderId );
        Task<Domain> GetDomainByOrderId( int orderId );
        Task<IList<Graphic>> GetGraphicByOrderId( int orderId );
    }
}
