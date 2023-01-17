using System;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.Models
{
    public class SearchResult
    {
        public SearchResult()
        {
            Page = 1;
            QuantityPerPage = 10;
            SortOrder = "Id";
            startDate = DateTime.Now;
            endDate = DateTime.Now.AddDays( 1 );
        }

        public int Page { get; set; }
        public int QuantityPerPage { get; set; }
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
    }

    public class SearchOption
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
