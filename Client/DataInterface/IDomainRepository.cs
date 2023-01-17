using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IDomainRepository
    {
        Task<IList<Domain>> GetAll();
        Task<Domain> GetOne( int id );
        Task<Domain> CreateDomain( Domain domain );
        Task<Domain> EditDomain( Domain domain );
        void DeleteDomain( int id );
    }
}
