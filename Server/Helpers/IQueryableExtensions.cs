using System.Linq;
using TakraonlineCRM.Shared.Models;

namespace TakraonlineCRM.Server.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>( this IQueryable<T> queryable,
            PaginationDTO pagination )
        {
            return queryable
                .Skip( (pagination.Page - 1) * pagination.QuantityPerPage )
                .Take( pagination.QuantityPerPage );
        }

        public static IQueryable<T> Paginate<T>( this IQueryable<T> queryable,
            SearchResult searchResult )
        {
            return queryable
                .Skip( (searchResult.Page - 1) * searchResult.QuantityPerPage )
                .Take( searchResult.QuantityPerPage );
        }
    }
}
