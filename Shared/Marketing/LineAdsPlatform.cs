using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.Marketing
{
    public class LineAdsPlatform : MarketingBase
    {
        public LineAdsPlatform()
        {
            _objective = new List<string>();
            _objective.Add( "Click Add Friend" );
            _objective.Add( "Click to Line OA" );
            _objective.Add( "Click to Website หรือ Sale Page" );
        }
        [Required( ErrorMessage = "กรุณาระบุ Line Official Account" )]
        public string LineOA { get; set; }
        public string TargetUrl { get; set; }
        public string SalePage { get; set; }
        public List<string> _objective;
    }
}
