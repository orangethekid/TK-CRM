using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.Marketing
{
    public class FacebookAds : MarketingBase
    {
        public FacebookAds() 
        {
            _objective = new List<string>();
            _objective.Add( "โปรโมทเพจ" );
            _objective.Add( "โปรโมทโพสต์" );
            _objective.Add( "ข้อความ" );
            _objective.Add( "คลิกไปเว็บไซต์หรือ Sale Page" );
        }
        [Required( ErrorMessage = "กรุณาระบุ Fabook Page" )]
        public string FacebookPage { get; set; }
        public string TargetUrl { get; set; }
        public string SalePage { get; set; }
        public List<string> _objective;
    }
}
