namespace TakraonlineCRM.Shared.Marketing
{
    public class GoogleShop : MarketingBase
    {
        public int WebSiteId { get; set; }
        public string MerchantID { get; set; }
        public string MerchantUserName { get; set; }
        public string MerchantPassword { get; set; }

        private string WebSiteName { get; set; }
        public void setWebsiteName( string id ) { WebSiteName = id; }
        public string getWebSiteName() { return WebSiteName; }

    }
}
