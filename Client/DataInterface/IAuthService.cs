using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IAuthService
    {
        Task Login( LoginRequest loginRequest );
        Task Register( UserRequest userRequest );
        Task Logout();
        Task<CurrentUser> CurrentUserInfo();
        Task<(int totalPageQuantity, IList<User> userList)> Get( int page = 1, int quantityPerPage = 10 );
        Task<IList<User>> GetAll();
        Task<IList<User>> GetAllByRole( string roleName );
        Task<UserEditRequest> EditUser( UserEditRequest userRequest );
        Task<User> GetOneByUserId( Guid userId );
        Task<User> GetOneByEmail( string email );
        Task<User> GetOneByUserName( string userName );
        Task DeleteUser( Guid userId );
    }
}
