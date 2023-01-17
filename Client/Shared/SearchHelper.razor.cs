using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using TakraonlineCRM.Shared.Models;

namespace TakraonlineCRM.Client.Shared
{
    public partial class SearchHelper
    {
        [Parameter] public List<SearchOption> searchOptions { get; set; }
        [Parameter] public SearchResult search { get; set; }
        [Parameter] public EventCallback OnSearch { get; set; }

        protected void ChangeSearchOption( ChangeEventArgs e )
        {
            search.CurrentFilter = e.Value.ToString();
            search.SearchString = string.Empty;
        }
    }
}
