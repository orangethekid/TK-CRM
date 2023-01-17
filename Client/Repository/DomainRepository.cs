using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TakraonlineCRM.Shared.WebSites;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Repository
{
    public class DomainRepository : IDomainRepository
    {
        private readonly HttpClient _httpClient;
        public DomainRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }
        #region Private
        #endregion

        #region Public
        public async Task<IList<Domain>> GetAll()
        {
            IList<Domain> domains = new List<Domain>();
            domains = await _httpClient.GetFromJsonAsync<List<Domain>>( "api/Domain/Getall" );

            return domains;
        }
        public async Task<Domain> GetOne( int id )
        {
            Domain domain = new Domain();
            domain = await _httpClient.GetFromJsonAsync<Domain>( "api/Domain/GetOne?id=" + id );

            return domain;
        }
        public async Task<Domain> CreateDomain( Domain domain )
        {
            await _httpClient.PostAsJsonAsync( "api/Domain/CreateDomain", domain );
            return domain;
        }
        public async Task<Domain> EditDomain( Domain domain )
        {
            await _httpClient.PutAsJsonAsync( "api/Domain/EditDomain", domain );
            return domain;
        }
        public async void DeleteDomain( int id )
        {
            await _httpClient.DeleteAsync( $"api/Domain/DeleteDomain?id={id}" );
        }
        #endregion
    }
}
