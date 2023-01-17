using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.Services;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.Authentication
{
    public partial class Login
    {
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] CustomStateProvider authStateProvider { get; set; }

        public LoginRequest loginRequest { get; set; } = new LoginRequest();
        public string error { get; set; }

        protected async Task OnSubmit()
        {
            error = null;
            try
            {
                await authStateProvider.Login( loginRequest );
                navigationManager.NavigateTo( "" );
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}
