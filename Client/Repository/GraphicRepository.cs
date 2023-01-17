using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Shared.Graphics;

namespace TakraonlineCRM.Client.Repository
{
    public class GraphicRepository : IGraphicRepository
    {
        private readonly HttpClient _httpClient;
        public GraphicRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region
        public async Task<IList<Graphic>> GetByOrderId( int OrderId )
        {
            IList<Graphic> graphics = new List<Graphic>();
            graphics = await _httpClient.GetFromJsonAsync<IList<Graphic>>( "api/Graphic/GetByOrderId?OrderId=" + OrderId );
            return graphics;
        }
        public async Task<Graphic> GetByGraphicId( int graphicID )
        {
            Graphic graphic = new Graphic();
            graphic = await _httpClient.GetFromJsonAsync<Graphic>( "api/Graphic/GetByGraphicId?graphicID=" + graphicID );
            return graphic;
        }
        public async Task<Graphic> CreateGraphic( Graphic graphic )
        {
            var response = await _httpClient.PostAsJsonAsync( "api/Graphic/CreateGraphic", graphic );
            graphic.Id = response.Content.ReadFromJsonAsync<int>().Result;
            return graphic;
        }
        public async Task<Graphic> EditGraphic( Graphic graphic )
        {
            await _httpClient.PutAsJsonAsync( "api/Graphic/EditGraphic", graphic );
            return graphic;
        }
        public async void DeleteGraphic(int id)
        {
            await _httpClient.DeleteAsync( $"api/Graphic/DeleteGraphic?id={id}" );
        }
        #endregion
    }
}
