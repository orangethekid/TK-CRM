using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IUserRepository
    {
        Task<string> GetUserDisplayNameById( string id );
        Task<IList<User>> GetUserList();
        Task<IList<User>> GetUserListByRole( string role );
    }
}
