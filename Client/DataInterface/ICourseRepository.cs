using System.Collections.Generic;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Setting.Course;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface ICourseRepository
    {
        Task<IList<Course>> GetAll();
        Task<(int totalPageQuantity, IList<Course> courseList)> Get( int page = 1, int quantityPerPage = 10 );
        Task<Course> GetByCourseId( int courseID );
        Task<Course> CreateCourse( Course course );
        Task<Course> EditCourse( Course course );
        void DeleteCourse( int id );
    }
}
