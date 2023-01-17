using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.User
{
    public partial class UserList
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        public IList<TakraonlineCRM.Shared.User.User> Users { get; set; }
        private CurrentUser currentUser = new CurrentUser();
        private ActivitiesLog activitiesLog = new ActivitiesLog();
        private int totalPagesQuantity;
        private int currentPage = 1;

        #region private
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
                    if ((currentUser.Role.ToLower() != "admin") && (currentUser.Role.ToLower() != "subadmin"))
                    {
                        uriHelper.NavigateTo( "/index" );
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private async Task PopulateControl()
        {
            try
            {
                var getUser = await auth.Get();
                totalPagesQuantity = getUser.totalPageQuantity;
                Users = getUser.userList;
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = currentUser.Id;
            activitiesLog.UserDisplayName = currentUser.DisplayName;
            activitiesLog.UserRole = currentUser.Role;
            activitiesLog.PageAction = "User List";
        }
        #endregion

        #region protected
        protected override async Task OnInitializedAsync()
        {
            await AuthenticateCheck();
            await PopulateControl();
            PopulateLog();
        }
        protected async Task SelectedPage( int page )
        {
            currentPage = page;
            var getUser = await auth.Get( page );
            totalPagesQuantity = getUser.totalPageQuantity;
            Users = getUser.userList;
        }
        protected async Task Delete( Guid deleteUserId )
        {
            var _user = Users.First( x => x.Id == deleteUserId.ToString() );
            if (await js.InvokeAsync<bool>( "confirm", $"คุณต้องการลบผู้ใช้ {_user.DisplayName} ({_user.UserName})?" ))
            {
                await auth.DeleteUser( deleteUserId );
                activitiesLog.Actionlog = $"ลบผู้ใช้งาน {_user.DisplayName} {_user.Role}";
                activitiesLog.BackupObject = JsonConvert.SerializeObject( _user );
                await log.CreateLog( activitiesLog );
                await OnInitializedAsync();
            }
        }
        protected bool admindisable( string username )
        {
            return username == "admin";
        }
        #endregion
    }
}
