using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Client.Pages.User
{
    public partial class Create
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IAuthService auth { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        public CurrentUser currentUser = new CurrentUser();
        public UserRequest userRequest { get; set; } = new UserRequest();
        private string statusMessage;
        private string statusClass;

        #region private
        private bool ValidatePassword( string password )
        {
            if (password.Length < 1)
            {
                statusClass = "alert-danger";
                statusMessage = "กรุณาระบุรหัสผ่าน";
                return false;
            }
            if (password.Length < 8)
            {
                statusClass = "alert-danger";
                statusMessage = "ความยาวของรหัสผ่านต้องไม่ต่ำกว่า 8 ตัวอักษร";
                return false;
            }
            if (!Regex.Match( password, @"\d+", RegexOptions.ECMAScript ).Success)
            {
                statusClass = "alert-danger";
                statusMessage = "รหัสผ่านต้องมีตัวเลขอย่างน้อย 1 ตัวอักษร";
                return false;
            }
            if (!(Regex.Match( password, @"[a-z]" ).Success && Regex.Match( password, @"[A-Z]" ).Success))
            {
                statusClass = "alert-danger";
                statusMessage = "รหัสผ่านต้องมีตัวพิมพ์ใหญ่อย่างน้อย 1 ตัวอักษร, รหัสผ่านต้องมีตัวพิมพ์เล็กอย่างน้อย 1 ตัวอักษร";
                return false;
            }
            if (!Regex.Match( password, @"[!@#$%^&*?_~£()-]" ).Success)
            {
                statusClass = "alert-danger";
                statusMessage = "รหัสผ่านต้องมีตัวอักขระพิเศษอย่างน้อย 1 ตัวอักษร (!@#$%^&*?_~-£())";
                return false;
            }

            statusMessage = string.Empty;
            statusClass = string.Empty;
            return true;
        }
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
                        uriHelper.NavigateTo( "/" );
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private async Task<bool> DuplicateNameOrEmail( UserRequest userRequest )
        {
            bool nameDuplicate = false;
            bool emailDuplicate = false;
            try
            {
                TakraonlineCRM.Shared.User.User username = await auth.GetOneByUserName( userRequest.UserName );
                if (username.UserName is not null)
                {
                    nameDuplicate = true;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );

            }

            try
            {
                TakraonlineCRM.Shared.User.User email = await auth.GetOneByEmail( userRequest.Email );
                if (email.Email is not null)
                {
                    emailDuplicate = true;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );

            }

            if (nameDuplicate)
            {
                if (emailDuplicate)
                {
                    statusClass = "alert-danger";
                    statusMessage = "มีบัญชีผู้ใช้และอีเมลนี้อยู่แล้วในระบบ";
                }
                else
                {
                    statusClass = "alert-danger";
                    statusMessage = "มีบัญชีผู้ใช้นี้อยู่แล้วในระบบ";
                }
            }
            else if (emailDuplicate)
            {
                statusClass = "alert-danger";
                statusMessage = "มีอีเมลนี้อยู่แล้วในระบบ";
            }
            else
            {
                statusMessage = string.Empty;
                statusClass = string.Empty;
            }


            return nameDuplicate || emailDuplicate;
        }
        #endregion

        #region protected
        protected override async Task OnParametersSetAsync()
        {
            await AuthenticateCheck();
        }
        protected async Task CreateUser()
        {
            if ((currentUser.Role.ToLower() == "admin") || (currentUser.Role.ToLower() == "subadmin"))
            {
                if (ValidatePassword( userRequest.Password ))
                {
                    bool duplicate = await DuplicateNameOrEmail( userRequest );
                    if (!duplicate)
                    {
                        try
                        {
                            await auth.Register( userRequest );
                            uriHelper.NavigateTo( "User" );
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine( error );
                        }
                    }
                }
            }
        }
        #endregion
    }
}
