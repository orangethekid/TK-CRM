using System;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.WebSites
{
    public class Domain : BaseEntity
    {
        public Domain()
        {
            CreateDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears( 1 );
            SSLCreateDate = DateTime.Now;
            SSLEndDate = DateTime.Now.AddYears( 1 );
        }

        public int WebSiteId { get; set; }
        public int OrderId { get; set; }
        [Required(ErrorMessage ="กรุณาระบุ Url เว็บไซต์")] 
        public string Name { get; set; }
        public bool IsOwn { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSSL { get; set; }
        public DateTime SSLCreateDate { get; set; }
        public DateTime SSLEndDate { get; set; }
    }
}
