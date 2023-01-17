using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;
        public UserRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Public
        public async Task<string> GetUserDisplayNameById( string userid )
        {
            User user = new User();
            user = await _httpClient.GetFromJsonAsync<User>( "api/User/GetUserDisplayNameById?userid=" + userid );          
            return user.DisplayName;
        }
        public async Task<IList<User>> GetUserList()
        {
            IList<User> list = new List<User>();
            list = await _httpClient.GetFromJsonAsync<List<User>>( "api/User/GetUserList" );
            
            return list;
        }
        public async Task<IList<User>> GetUserListByRole( string role )
        {
            IList<User> list = new List<User>();
            list = await _httpClient.GetFromJsonAsync<List<User>>( "api/User/GetUserListByRole?role=" + role );

            return list;
        }
        #endregion
    }
}
