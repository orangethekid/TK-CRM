using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.Setting.Course
{
    public class Course : BaseEntity
    {
        [Required( ErrorMessage = "กรุณาระบุชื่อคอร์สเรียน" )]
        public string CourseName { get; set; }

        public string CourseDetail { get; set; }
    }
}
