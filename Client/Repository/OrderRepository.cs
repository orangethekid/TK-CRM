using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Graphics;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;
        public OrderRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Private
        private async Task<Order> CreateOrder( Order order )
        {
            var response = await _httpClient.PostAsJsonAsync( "api/Order/CreateOrder", order );
            order.Id = response.Content.ReadFromJsonAsync<int>().Result;
            return order;
        }
        #endregion

        #region Public
        public async Task<(int totalPageQuantity, IList<Order> orderList)> Get( int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Order/Get?page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var customerList = JsonSerializer.Deserialize<List<Order>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, customerList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<(int totalPageQuantity, IList<Order> orderList)> Get( SearchResult result )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Order/Search?page={result.Page}&quantityPerPage={result.QuantityPerPage}" +
                                                               $"&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}" +
                                                               $"&startDate={result.startDate}&endDate={result.endDate}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var orderList = JsonSerializer.Deserialize<List<Order>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, orderList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<IList<Order>> GetAll()
        {
            IList<Order> list = new List<Order>();
            list = await _httpClient.GetFromJsonAsync<List<Order>>( "api/Order/GetAll" );

            return list;
        }
        public async Task<(int totalPageQuantity, IList<Order> orderList)> GetByCustomerId( int customerId = 1, int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Order/GetByCustomerId?customerId={customerId}&page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var orderList = JsonSerializer.Deserialize<List<Order>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, orderList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<IList<Order>> GetAllByCustomerId( int customerId )
        {
            IList<Order> list = new List<Order>();
            list = await _httpClient.GetFromJsonAsync<List<Order>>( "api/Order/GetAllByCustomerId?customerId=" + customerId );

            return list;
        }
        public async Task<(int totalPageQuantity, IList<Order> orderList)> GetByCretorId( string creatorId = "1", int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Order/GetByCretorId?creatorId={creatorId}&page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var orderList = JsonSerializer.Deserialize<List<Order>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, orderList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<IList<Order>> GetAllByCretorId( string creatorId )
        {
            IList<Order> list = new List<Order>();
            list = await _httpClient.GetFromJsonAsync<List<Order>>( "api/Order/GetAllByCretorId?creatorId=" + creatorId );

            return list;
        }
        public async Task<(int totalPageQuantity, IList<Order> orderList)> GetBySaleId( int page, int quantityPerPage, string saleId )
        {
            SearchResult search = new SearchResult();
            search.Page = page;
            search.QuantityPerPage = quantityPerPage;
            return await GetBySaleId( search, saleId );
        }
        public async Task<(int totalPageQuantity, IList<Order> orderList)> GetBySaleId( SearchResult result, string saleId )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Order/GetBySaleId?page={result.Page}&quantityPerPage={result.QuantityPerPage}" +
                                                                          $"&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}&saleId={saleId}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var orderList = JsonSerializer.Deserialize<List<Order>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, orderList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<Order> GetOne( int id )
        {
            Order order = new Order();
            order = await _httpClient.GetFromJsonAsync<Order>( "api/Order/GetOne?id=" + id );

            return order;
        }

        public async Task<Order> GetMaxTakraOrderID()
        {
            Order order = new Order();
            order = await _httpClient.GetFromJsonAsync<Order>( "api/Order/GetMaxTakraOrderID" );

            return order;
        }

        public async Task<Order> CreateOrder( Order order, int customerId, string creatorId )
        {
            Order _order = new Order();
            _order = order;
            _order.CustomerId = customerId;
            _order.CreatorId = creatorId;
            return await CreateOrder( _order );
        }
        public async Task<Order> EditOrder( Order order )
        {
            await _httpClient.PutAsJsonAsync( "api/Order/EditOrder", order );
            return order;
        }
        public async void Delete( int id )
        {
            await _httpClient.DeleteAsync( $"api/Order/Delete?id={id}" );
        }

        public async Task<WebSite> GetWebSiteByOrderId( int orderId )
        {
            WebSite web = await _httpClient.GetFromJsonAsync<WebSite>( "api/Order/GetWebSiteByOrderID?orderId=" + orderId );
            return web;
        }
        public async Task<Domain> GetDomainByOrderId( int orderId )
        {
            Domain domain = await _httpClient.GetFromJsonAsync<Domain>( "api/Order/GetDomainByOrderID?orderId=" + orderId );
            return domain;
        }
        public async Task<IList<Graphic>> GetGraphicByOrderId( int orderId )
        {
            IList<Graphic> graphics = await _httpClient.GetFromJsonAsync<List<Graphic>>( "api/Order/GetGraphicByOrderID?orderId=" + orderId );
            return graphics;
        }
        #endregion
    }
}
