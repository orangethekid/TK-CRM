using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.Repository
{
    public class WebSiteRepository : IWebSiteRepository
    {
        private readonly HttpClient _httpClient;
        public WebSiteRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Private
        private async Task<WebSite> CreateWebSite( WebSite webSite )
        {
            var response = await _httpClient.PostAsJsonAsync( "api/WebSite/CreateWebSite", webSite );
            webSite.Id = response.Content.ReadFromJsonAsync<int>().Result;
            return webSite;
        }
        #endregion

        #region Public
        public async Task<(int totalPageQuantity, IList<WebSite> websiteList)> Get( int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Website/Get?page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var websiteList = JsonSerializer.Deserialize<List<WebSite>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, websiteList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<(int totalPageQuantity, IList<WebSite> websiteList)> Get( SearchResult result )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Website/Search?page={result.Page}&quantityPerPage={result.QuantityPerPage}" +
                                                                           $"&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var websiteList = JsonSerializer.Deserialize<List<WebSite>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, websiteList);
            }
            else
            {
                return (0, null);
            }
        }

        public async Task<IList<WebSite>> GetAll()
        {
            IList<WebSite> websites = new List<WebSite>();
            websites = await _httpClient.GetFromJsonAsync<List<WebSite>>( "api/WebSite/Getall" );

            return websites;
        }
        public async Task<IList<WebSite>> GetAllByCustomer( int customerId )
        {
            IList<WebSite> websites = new List<WebSite>();
            websites = await _httpClient.GetFromJsonAsync<List<WebSite>>( "api/WebSite/GetAllByCustomer?customerId=" + customerId );

            return websites;
        }
        public async Task<WebSite> GetOne( int id )
        {
            WebSite webstite = new WebSite();
            webstite = await _httpClient.GetFromJsonAsync<WebSite>( "api/WebSite/GetOne?Id=" + id );

            return webstite;
        }
        public async Task<WebSite> CreateWebSite( WebSite webSite, int customerId, int orderId )
        {
            WebSite _webSite = new WebSite();
            _webSite = webSite;
            _webSite.CustomerId = customerId;
            _webSite.OrderId = orderId;

            return await CreateWebSite( _webSite );
        }
        public async Task<WebSite> CreateWebSite( WebSite webSite, int customerId )
        {
            WebSite _webSite = new WebSite();
            _webSite = webSite;
            _webSite.CustomerId = customerId;

            return await CreateWebSite( _webSite );
        }
        public async Task<WebSite> EditWebSite( WebSite webSite )
        {
            await _httpClient.PutAsJsonAsync( "api/WebSite/EditWebSite", webSite );
            return webSite;
        }
        public async void DeleteWebSite( int id )
        {
            await _httpClient.DeleteAsync( $"api/WebSite/DeleteWebSite?id={id}" );
        }
        #endregion
    }
}
