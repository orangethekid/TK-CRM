using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task<CurrentUser> CurrentUserInfo()
        {
            var result = await _httpClient.GetFromJsonAsync<CurrentUser>( "api/auth/currentuserinfo" );
            return result;
        }

        public async Task Login( LoginRequest loginRequest )
        {
            var result = await _httpClient.PostAsJsonAsync( "api/auth/login", loginRequest );
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception( await result.Content.ReadAsStringAsync() );

            Console.WriteLine( "Login code =" + result.StatusCode );

            result.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync( "api/auth/logout", null );
            result.EnsureSuccessStatusCode();
        }

        public async Task Register( UserRequest userRequest )
        {
            var result = await _httpClient.PostAsJsonAsync( "api/auth/register", userRequest );
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception( await result.Content.ReadAsStringAsync() );
            result.EnsureSuccessStatusCode();
        }

        public async Task<IList<User>> GetAll()
        {
            IList<User> users = await _httpClient.GetFromJsonAsync<List<User>>( "api/auth/GetAll" );

            return users;
        }

        public async Task<(int totalPageQuantity, IList<User> userList)> Get( int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/auth/Get?page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var userList = JsonSerializer.Deserialize<List<User>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, userList);
            }
            else
            {
                return (0, null);
            }
        }

        public async Task<IList<User>> GetAllByRole( string roleName )
        {
            IList<User> users = await _httpClient.GetFromJsonAsync<List<User>>( $"api/auth/GetAllByRole?roleName={roleName}" );

            return users;
        }

        public async Task<User> GetOneByUserId( Guid userId )
        {
            User user = await _httpClient.GetFromJsonAsync<User>( $"api/auth/GetOneByUserId?userId={userId}" );

            return user;
        }

        public async Task<User> GetOneByEmail( string email )
        {
            User user = await _httpClient.GetFromJsonAsync<User>( "api/auth/GetOneByEmail?email=" + email );

            return user;
        }

        public async Task<User> GetOneByUserName( string userName )
        {
            User user = await _httpClient.GetFromJsonAsync<User>( "api/auth/GetOneByUserName?userName=" + userName );

            return user;
        }

        public async Task<UserEditRequest> EditUser( UserEditRequest userEditRequest )
        {
            await _httpClient.PutAsJsonAsync( "api/auth/EditUser", userEditRequest );

            return userEditRequest;
        }

        public async Task DeleteUser( Guid userId )
        {
            await _httpClient.DeleteAsync( $"api/auth/DeleteUser?userId={userId}" );
        }
    }
}
