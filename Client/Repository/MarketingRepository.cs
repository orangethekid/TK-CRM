using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Client.DataInterface;

namespace TakraonlineCRM.Client.DataInterface
{
    public class MarketingRepository : IMarketingRepository
    {
        private readonly HttpClient _httpClient;
        public MarketingRepository( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        #region Public
        public async Task<IList<FacebookAds>> GetFacebookAds()
        {
            IList<FacebookAds> list = new List<FacebookAds>();
            list = await _httpClient.GetFromJsonAsync<List<FacebookAds>>( "api/Marketing/GetAll?type=facebook" );
            return list;
        }
        public async Task<FacebookAds> GetFacebookAds( int id )
        {
            FacebookAds fb = new FacebookAds();
            fb = await _httpClient.GetFromJsonAsync<FacebookAds>( "api/Marketing/GetMarketingFacebook?id=" + id );
            return fb;
        }
        public async Task<FacebookAds> CreateFacebookAds( FacebookAds facebookAds )
        {
            await _httpClient.PostAsJsonAsync( "api/Marketing/CreateMarketingFacebook", facebookAds );
            return facebookAds;
        }
        public async Task<FacebookAds> EditFacebookAds( FacebookAds facebookAds )
        {
            await _httpClient.PutAsJsonAsync( "api/Marketing/EditMarketingFacebook", facebookAds );
            return facebookAds;
        }
        public async void DeleteFacebookAds( int id ) 
        {
            await _httpClient.DeleteAsync( $"api/Marketing/DeleteMarketingFacebook?id={id}" );
        }

        public async Task<IList<LineAdsPlatform>> GetLineAdsPlatform()
        {
            IList<LineAdsPlatform> list = new List<LineAdsPlatform>();
            list = await _httpClient.GetFromJsonAsync<List<LineAdsPlatform>>( "api/Marketing/GetAll?type=line" );
            return list;
        }
        public async Task<LineAdsPlatform> GetLineAdsPlatform( int id )
        {
            LineAdsPlatform line = new LineAdsPlatform();
            line = await _httpClient.GetFromJsonAsync<LineAdsPlatform>( "api/Marketing/GetMarketingLine?id=" + id );
            return line;
        }
        public async Task<LineAdsPlatform> CreateLineAdsPlatform( LineAdsPlatform lineAdsPlatform )
        {
            await _httpClient.PostAsJsonAsync( "api/Marketing/CreateMarketingLine", lineAdsPlatform );
            return lineAdsPlatform;
        }
        public async Task<LineAdsPlatform> EditLineAdsPlatform( LineAdsPlatform lineAdsPlatform )
        {
            await _httpClient.PutAsJsonAsync( "api/Marketing/EditMarketingLine", lineAdsPlatform );
            return lineAdsPlatform;
        }
        public async void DeleteLineAdsPlatform( int id )
        {
            await _httpClient.DeleteAsync( $"api/Marketing/DeleteMarketingLine?id={id}" );
        }

        public async Task<IList<GoogleShop>> GetGoogleShop()
        {
            IList<GoogleShop> list = new List<GoogleShop>();
            list = await _httpClient.GetFromJsonAsync<List<GoogleShop>>( "api/Marketing/GetAll?type=google" );
            return list;
        }
        public async Task<GoogleShop> GetGoogleShop( int id )
        {
            GoogleShop googleShop = new GoogleShop();
            googleShop = await _httpClient.GetFromJsonAsync<GoogleShop>( "api/Marketing/GetMarketingGoogle?id=" + id );
            return googleShop;
        }
        public async Task<GoogleShop> CreateGoogleShop( GoogleShop googleShop )
        {
            await _httpClient.PostAsJsonAsync( "api/Marketing/CreateMarketingGoogle", googleShop );
            return googleShop;
        }
        public async Task<GoogleShop> EditGoogleShop( GoogleShop googleShop ) 
        {
            await _httpClient.PutAsJsonAsync( "api/Merketing/EditMarketingGoogle", googleShop );
            return googleShop;
        }
        public async void DeleteGoogleShop( int id )
        {
            await _httpClient.DeleteAsync( $"api/Marketing/DeleteMarketingGoogle?id={id}" );
        }
        #endregion
    }
}
