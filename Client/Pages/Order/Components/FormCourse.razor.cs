using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Orders;

namespace TakraonlineCRM.Client.Pages.Order.Components
{
    public partial class FormCourse
    {
        [Inject] ICourseRepository courseRepository { get; set; }
        [Parameter] public OrderCourse OrderCourse { get; set; }
        private IList<TakraonlineCRM.Shared.Setting.Course.Course> courses = new List<TakraonlineCRM.Shared.Setting.Course.Course>();
        private string coursedetail = string.Empty;

        private async Task OnChangeAsync( ChangeEventArgs args )
        {
            var select = args.Value.ToString();
            TakraonlineCRM.Shared.Setting.Course.Course courseTemp = await courseRepository.GetByCourseId( Int32.Parse( select ) );

            coursedetail = courseTemp.CourseDetail;
            OrderCourse.CourseName = courseTemp.CourseName;
        }

        private async Task PopulateControl()
        {
            try
            {
                courses = await courseRepository.GetAll();
            }
            catch (Exception error)
            {
                Console.WriteLine( error );
            }

            if (OrderCourse.CourseName is not null)
            {

            }
        }

        protected async override Task OnParametersSetAsync()
        {
            await PopulateControl();
        }
    }
}
