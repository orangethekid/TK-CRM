using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;

namespace TakraonlineCRM.Client.Pages.ActivitiesLogs
{
    public partial class LogsList
    {
        [Inject] ILogRepository repository { get; set; }

        public IList<ActivitiesLog> logs { get; set; }
        public SearchResult search = new SearchResult() { Page = 1, CurrentFilter = "" };
        private List<SearchOption> searchField = new List<SearchOption>();
        public int totalPagesQuantity;
        private int currentPage = 1;

        private async Task PopulateControl()
        {
            try
            {
                await SelectedPage( 1 );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private void SetupSearchFilter()
        {
            searchField.Add( new SearchOption() { Key = "userdisplayname", Name = "ชื่อ" } );
            searchField.Add( new SearchOption() { Key = "userrole", Name = "ตำแหน่ง" } );
            searchField.Add( new SearchOption() { Key = "date", Name = "วันที่" } );
        }

        protected override async Task OnInitializedAsync()
        {
            await PopulateControl();
            SetupSearchFilter();
        }
        private async Task SelectedPage( int page )
        {
            currentPage = page;
            search.Page = page;
            var getLogs = await repository.Get( search );
            totalPagesQuantity = getLogs.totalPageQuantity;
            logs = getLogs.logs;
        }
        protected async Task Search()
        {
            await SelectedPage( 1 );
        }
    }
}
