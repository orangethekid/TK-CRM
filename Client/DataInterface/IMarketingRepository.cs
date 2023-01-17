using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Marketing;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IMarketingRepository
    {
        Task<IList<FacebookAds>> GetFacebookAds();
        Task<FacebookAds> GetFacebookAds( int id );
        Task<FacebookAds> CreateFacebookAds( FacebookAds facebookAds );
        Task<FacebookAds> EditFacebookAds( FacebookAds facebookAds );
        void DeleteFacebookAds( int id );

        Task<IList<LineAdsPlatform>> GetLineAdsPlatform();
        Task<LineAdsPlatform> GetLineAdsPlatform( int id );
        Task<LineAdsPlatform> CreateLineAdsPlatform( LineAdsPlatform lineAdsPlatform );
        Task<LineAdsPlatform> EditLineAdsPlatform( LineAdsPlatform lineAdsPlatform );
        void DeleteLineAdsPlatform( int id );

        Task<IList<GoogleShop>> GetGoogleShop();
        Task<GoogleShop> GetGoogleShop( int id );
        Task<GoogleShop> CreateGoogleShop( GoogleShop googleShop );
        Task<GoogleShop> EditGoogleShop( GoogleShop googleShop );
        void DeleteGoogleShop( int id );
    }
}
