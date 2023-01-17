using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IWebSiteRepository
    {
        Task<(int totalPageQuantity, IList<WebSite> websiteList)> Get( int page = 1, int quantityPerPage = 10 );
        Task<(int totalPageQuantity, IList<WebSite> websiteList)> Get( SearchResult result );
        Task<IList<WebSite>> GetAll();
        Task<IList<WebSite>> GetAllByCustomer( int customerId );
        Task<WebSite> GetOne( int id );
        Task<WebSite> CreateWebSite( WebSite webSite, int customerId, int orderId );
        Task<WebSite> CreateWebSite( WebSite webSite, int customerId );
        Task<WebSite> EditWebSite( WebSite webSite );
        void DeleteWebSite( int id );
    }
}
