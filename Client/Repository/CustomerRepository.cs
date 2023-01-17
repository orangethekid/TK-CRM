using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.Models;

namespace TakraonlineCRM.Client.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HttpClient _httpClient;
        public CustomerRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Private
        private async Task<Customer> CreateCustomer( Customer customer )
        {
            await _httpClient.PostAsJsonAsync( "api/Customer/CreateCustomer", customer );
            return customer;
        }
        #endregion

        #region Public
        public async Task<(int totalPageQuantity, IList<Customer> customerList)> Get( int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Customer/Get?page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var customerList = JsonSerializer.Deserialize<List<Customer>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, customerList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<(int totalPageQuantity, IList<Customer> customerList)> Get(SearchResult result )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Customer/Search?page={result.Page}&quantityPerPage={result.QuantityPerPage}" +
                                                                           $"&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var customerList = JsonSerializer.Deserialize<List<Customer>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, customerList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<IList<Customer>> GetAll()
        {
            IList<Customer> list = new List<Customer>();
            list = await _httpClient.GetFromJsonAsync<List<Customer>>( "api/Customer/GetAll" );

            return list;
        }
        public async Task<IList<Customer>> GetAllByCreatorId( string creatorId )
        {
            IList<Customer> list = new List<Customer>();
            list = await _httpClient.GetFromJsonAsync<List<Customer>>( "api/Customer/GetAllByCreatorId?creatorId=" + creatorId );

            return list;
        }
        public async Task<IList<Customer>> GetAllBySaleId( string saleId )
        {
            IList<Customer> list = new List<Customer>();
            list = await _httpClient.GetFromJsonAsync<List<Customer>>( "api/Customer/GetAllBySellId?sellId=" + saleId );

            return list;
        }
        public async Task<(int totalPageQuantity, IList<Customer> customerList)> GetAllBySaleId( int page , int quantityPerPage,string saleId )
        {
            SearchResult searchResult = new SearchResult();
            searchResult.Page = page;
            searchResult.QuantityPerPage = quantityPerPage;
            return await GetAllBySaleId( searchResult, saleId);
        }
        public async Task<(int totalPageQuantity, IList<Customer> customerList)> GetAllBySaleId( SearchResult result, string saleId )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Customer/SearchBySaleId?page={result.Page}&quantityPerPage={result.QuantityPerPage}" +
                                                                           $"&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}&saleId={saleId}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var customerList = JsonSerializer.Deserialize<List<Customer>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, customerList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<Customer> GetOneById( int customerId )
        {
            Customer customer = new Customer();
            customer = await _httpClient.GetFromJsonAsync<Customer>( "api/Customer/GetOneById?id=" + customerId );

            return customer;
        }
        public async Task<Customer> CreateCustomer( Customer customer, string creatorId, string saleId )
        {
            Customer _customer = customer;
            _customer.CreatorId = creatorId;
            _customer.SaleId = saleId;

            return await CreateCustomer( _customer );
        }
        public async Task<Customer> CreateCustomer( Customer customer, string creatorId )
        {
            Customer _customer = customer;
            _customer.CreatorId = creatorId;
            _customer.SaleId = string.Empty;

            return await CreateCustomer( _customer );
        }
        public async Task<Customer> EditCustomer( Customer customer )
        {
            await _httpClient.PutAsJsonAsync( "api/Customer/EditCustomer", customer );
            return customer;
        }
        public async void DeleteCustomer( int customerId )
        {
            await _httpClient.DeleteAsync( $"api/Customer/DeleteCustomer?id={customerId}" );
        }
        #endregion
    }
}
