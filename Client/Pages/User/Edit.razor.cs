using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.User;
using TakraonlineCRM.Shared.Models;
using Newtonsoft.Json;

namespace TakraonlineCRM.Client.Pages.User
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] IAuthService auth { get; set; }
        [Inject] ILogRepository log { get; set; }

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }

        [Parameter] public Guid userId { get; set; }
        public UserEditRequest userEditRequest = new UserEditRequest();
        private CurrentUser user = new CurrentUser();
        private ActivitiesLog activitiesLog = new ActivitiesLog();
        private string statusMessage;
        private string statusClass;

        #region private
        private async Task PopulateControl()
        {
            try
            {
                TakraonlineCRM.Shared.User.User user = await auth.GetOneByUserId( userId );
                if (user != null)
                {
                    userEditRequest = new UserEditRequest( user );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        private bool ValidatePassword( string password )
        {
            if (!string.IsNullOrEmpty( password ))
            {
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
            }

            statusMessage = string.Empty;
            statusClass = string.Empty;
            return true;
        }
        private void PopulateLog()
        {
            activitiesLog.UserId = user.Id;
            activitiesLog.UserDisplayName = user.DisplayName;
            activitiesLog.UserRole = user.Role;
            activitiesLog.PageAction = "User Edit";
            activitiesLog.Actionlog = $"แก้ไขผู้ใช้งาน { user.DisplayName}";
            activitiesLog.BackupObject = JsonConvert.SerializeObject( user );
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await PopulateControl();
            PopulateLog();
        }
        private async Task HandleValidSubmit()
        {
            bool duplicate = await DuplicateNameOrEmail( userEditRequest );
            if (!duplicate)
            {
                try
                {
                    if (ValidatePassword( userEditRequest.NewPassword ))
                    {
                        await auth.EditUser( userEditRequest );
                        await log.CreateLog( activitiesLog );
                        await js.InvokeVoidAsync( "alert", $"Updated Successfully!" );
                        uriHelper.NavigateTo( $"user/edit/{userId}" );
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine( error );
                }
            }

        }
        private async Task<bool> DuplicateNameOrEmail( UserEditRequest userEditRequest )
        {
            bool nameDuplicate = false;
            bool emailDuplicate = false;
            try
            {
                TakraonlineCRM.Shared.User.User username = await auth.GetOneByUserName( userEditRequest.UserName );
                if (username.UserName is not null)
                {
                    if (username.Id != userEditRequest.Id)
                        nameDuplicate = true;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine( error );

            }

            try
            {
                TakraonlineCRM.Shared.User.User email = await auth.GetOneByEmail( userEditRequest.Email );
                if (email.Email is not null)
                {
                    if (email.Id != userEditRequest.Id)
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
    }
}
