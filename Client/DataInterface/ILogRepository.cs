using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface ILogRepository
    {
        Task<(int totalPageQuantity, IList<ActivitiesLog> logs)> Get( int page = 1, int quantityPerPage = 10 );
        Task<(int totalPageQuantity, IList<ActivitiesLog> logs)> Get( SearchResult searchResult );
        Task<ActivitiesLog> CreateLog( ActivitiesLog logs );
    }
}
