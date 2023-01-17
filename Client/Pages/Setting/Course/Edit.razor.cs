using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Pages.Setting.Course
{
    public partial class Edit
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ICourseRepository repository { get; set; }

        [Parameter] public int courseId { get; set; }
        public TakraonlineCRM.Shared.Setting.Course.Course course = new TakraonlineCRM.Shared.Setting.Course.Course();

        #region private
        private async Task PopulateControl()
        {
            try
            {
                course = await repository.GetByCourseId( courseId );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion

        #region protected
        protected async override Task OnParametersSetAsync()
        {
            await PopulateControl();
        }
        protected async Task EditCourse()
        {
            try
            {
                await repository.EditCourse( course );
                await js.InvokeVoidAsync( "alert", $"บันทึกการแก้ไขเรียบร้อยแล้ว" );
                uriHelper.NavigateTo( "/Course" );
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion
    }
}
