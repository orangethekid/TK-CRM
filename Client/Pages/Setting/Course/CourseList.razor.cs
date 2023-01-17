using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.Pages.Setting.Course
{
    public partial class CourseList
    {
        [Inject] NavigationManager uriHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        [Inject] ICourseRepository repository { get; set; }

        private IList<TakraonlineCRM.Shared.Setting.Course.Course> Courses { get; set; }
        private int totalPagesQuantity;
        private int currentPage = 1;

        #region private


        private async Task PopulateControl()
        {
            try
            {
                var getCourse = await repository.Get();
                totalPagesQuantity = getCourse.totalPageQuantity;
                Courses = getCourse.courseList;
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }
        }
        #endregion

        #region protected
        protected override async Task OnInitializedAsync()
        {
            await PopulateControl();
        }
        protected async Task SelectedPage( int page )
        {
            currentPage = page;
            var getCourse = await repository.Get( page );
            totalPagesQuantity = getCourse.totalPageQuantity;
            Courses = getCourse.courseList;
        }
        protected async Task Delete( int courseId )
        {
            var course = Courses.First( x => x.Id == courseId );
            if (await js.InvokeAsync<bool>( "confirm", $"คุณต้องการลบคอร์สเรียน {course.CourseName} ?" ))
            {
                repository.DeleteCourse( courseId );
                await OnInitializedAsync();
            }
            uriHelper.NavigateTo( "/Course" );
        }
        protected bool admindisable( string username )
        {
            return username == "admin";
        }
        #endregion
    }
}