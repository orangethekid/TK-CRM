using System;

namespace TakraonlineCRM.Shared.Marketing
{
    public class MarketingBase : BaseEntity
    {
        public MarketingBase()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays( 7 );
        }

        public int OrderID { get; set; }
        public string Campaign { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Budget { get; set; }
        public string Objective { get; set; }

        protected string OrderTakraId = string.Empty;
        public void setOrderTakraId( string id) { OrderTakraId = id; }
        public string getOrderTakraId() { return OrderTakraId; }
    }
}
