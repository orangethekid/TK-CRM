using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Setting.Course
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ICourseRepository repository { get; set; }
        [Inject] IAuthService auth { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        public TakraonlineCRM.Shared.Setting.Course.Course currentcourse = new TakraonlineCRM.Shared.Setting.Course.Course();
        public CurrentUser user = new CurrentUser();

        #region private
        private async Task PopulateControl()
        {
            if (!(await AuthenticationState).User.Identity.IsAuthenticated)
            {
                uriHelper.NavigateTo( "/index" );
            }
            else
            {
                user = await auth.CurrentUserInfo();
            }
        }
        #endregion

        #region protected
        protected override async Task OnParametersSetAsync()
        {
            await PopulateControl();
        }

        protected async Task CreateCourse()
        {
            try
            {
                currentcourse = await repository.CreateCourse( currentcourse );
                await js.InvokeVoidAsync( "alert", $"เพิ่มคอร์สเรียนเรียบร้อยแล้ว" );
                uriHelper.NavigateTo( "course" );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
