using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly HttpClient _httpClient;
        public LogRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Public
        public async Task<(int totalPageQuantity, IList<ActivitiesLog> logs)> Get( int page = 1, int quantityPerPage = 10 )
        {
            SearchResult search = new SearchResult();
            search.Page = page;
            search.QuantityPerPage = quantityPerPage;
            return await Get( search );
        }
        public async Task<(int totalPageQuantity, IList<ActivitiesLog> logs)> Get( SearchResult result )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Log/Get?page={result.Page}&quantityPerPage={result.QuantityPerPage}&sortorder={result.SortOrder}&currentFilter={result.CurrentFilter}&searchstring={result.SearchString}&startDate={result.startDate}&endDate={result.endDate}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var LogList = JsonSerializer.Deserialize<List<ActivitiesLog>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, LogList);
            }
            else
            {
                return (0, null);
            }
        }
        public async Task<ActivitiesLog> CreateLog( ActivitiesLog logs )
        {
            await _httpClient.PostAsJsonAsync( "api/Log/CreateLog", logs );
            return logs;
        }
        #endregion
    }
}
