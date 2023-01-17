using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.WebSite
{
    public partial class WebsiteList
    {
        [Inject] IWebSiteRepository websiteRepository { get; set; }
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        //Search And Paging
        private SearchResult search = new SearchResult() { Page = 1, CurrentFilter = "name", SortOrder = "Descending" };
        private List<SearchOption> searchField = new List<SearchOption>();
        private int totalPagesQuantity;
        private int currentPage = 1;

        IList<TakraonlineCRM.Shared.WebSites.WebSite> WebSites { get; set; }
        private CurrentUser currentUser { get; set; }

        #region privatte
        private async Task AuthenticateCheck()
        {
            try
            {
                if (!(await AuthenticationState).User.Identity.IsAuthenticated)
                {
                    uriHelper.NavigateTo( "/index" );
                }
                else
                {
                    currentUser = await auth.CurrentUserInfo();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private void SetupSearchFilter()
        {
            searchField.Add( new SearchOption() { Key = "name", Name = "ชื่อเว็บไซต์" } );
            searchField.Add( new SearchOption() { Key = "url", Name = "URL" } );
            searchField.Add( new SearchOption() { Key = "version", Name = "Version" } );
            searchField.Add( new SearchOption() { Key = "maximumproduct", Name = "จำนวนสินค้า/SalePage" } );
        }

        private async Task PopulateControl()
        {
            await SelectedPage( 1 );
        }
        #endregion

        #region protected
        protected override async Task OnInitializedAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            SetupSearchFilter();
        }

        protected async Task SelectedPage( int page )
        {
            currentPage = page;
            search.Page = page;

            var getWebsite = await websiteRepository.Get( search );
            totalPagesQuantity = getWebsite.totalPageQuantity;
            WebSites = getWebsite.websiteList;
        }

        protected async Task Search()
        {
            await SelectedPage( 1 );
        }

        protected async Task Delete( int WebSitID )
        {
            var web = WebSites.First( x => x.Id == WebSitID );
            if (await js.InvokeAsync<bool>( "confirm", $"Do you want to delete {web.Id} Record?" ))
            {
                try
                {
                    websiteRepository.DeleteWebSite( web.Id );
                    uriHelper.NavigateTo( "/Web" );
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }
        }

        #endregion
    }
}