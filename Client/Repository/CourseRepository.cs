using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Setting.Course;

namespace TakraonlineCRM.Client.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly HttpClient _httpClient;
        public CourseRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region

        public async Task<IList<Course>> GetAll()
        {
            IList<Course> users = await _httpClient.GetFromJsonAsync<List<Course>>( "api/Course/GetAll" );

            return users;
        }

        public async Task<(int totalPageQuantity, IList<Course> courseList)> Get( int page = 1, int quantityPerPage = 10 )
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync( $"api/Course/Get?page={page}&quantityPerPage={quantityPerPage}" );
            if (httpResponse.IsSuccessStatusCode)
            {
                int totalPageQuantity = int.Parse( httpResponse.Headers.GetValues( "x-pagination" ).FirstOrDefault() );
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var userList = JsonSerializer.Deserialize<List<Course>>( responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true } );

                return (totalPageQuantity, userList);
            }
            else
            {
                return (0, null);
            }
        }

        public async Task<Course> GetByCourseId( int courseID )
        {
            Course course = new Course();
            course = await _httpClient.GetFromJsonAsync<Course>( "api/Course/GetByCourseId?CourseID=" + courseID );
            return course;
        }
        public async Task<Course> CreateCourse( Course course )
        {
            var response = await _httpClient.PostAsJsonAsync( "api/Course/CreateCourse", course );
            course.Id = response.Content.ReadFromJsonAsync<int>().Result;
            return course;
        }
        public async Task<Course> EditCourse( Course course )
        {
            await _httpClient.PutAsJsonAsync( "api/Course/EditCourse", course );
            return course;
        }
        public async void DeleteCourse( int id )
        {
            await _httpClient.DeleteAsync( $"api/Course/DeleteCourse?id={id}" );
        }
        #endregion
    }
}
