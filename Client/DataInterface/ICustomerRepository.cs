using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.Models;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface ICustomerRepository
    {
        Task<(int totalPageQuantity, IList<Customer> customerList)> Get( int page = 1, int quantityPerPage = 10 );
        Task<(int totalPageQuantity, IList<Customer> customerList)> Get( SearchResult result );
        Task<IList<Customer>> GetAll();
        Task<IList<Customer>> GetAllByCreatorId( string creatorId );
        Task<(int totalPageQuantity, IList<Customer> customerList)> GetAllBySaleId( int page,int quantityPerPage, string saleId );
        Task<(int totalPageQuantity, IList<Customer> customerList)> GetAllBySaleId( SearchResult result, string saleId );
        Task<IList<Customer>> GetAllBySaleId( string saleId );
        Task<Customer> GetOneById( int customerId );
        Task<Customer> CreateCustomer( Customer customer, string creatorId, string saleId );
        Task<Customer> CreateCustomer( Customer customer, string creatorId );
        Task<Customer> EditCustomer( Customer customer );
        void DeleteCustomer( int customerId );
    }
}
