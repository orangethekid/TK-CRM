using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Shared.Customers
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            CreateDate = DateTime.Now;
        }

        public string CreatorId { get; set; }
        private string CreatorName = string.Empty;
        public string SaleId { get; set; }
        private string SaleName = string.Empty;
        [Required( ErrorMessage = "กรุณาระบุชื่อ" )]
        public string FirstName { get; set; }
        [Required( ErrorMessage = "กรุณาระบุนามสกุล" )]
        public string LastName { get; set; }
        [Required( ErrorMessage = "กรุณาระบุประเภทธุรกิจ" )]
        public string BusinessType { get; set; }
        public string BusinessName { get; set; }
        public string BusinessContact { get; set; }

        public string CustomerDetail { get; set; }
        public string CustomerNote { get; set; }
        [Required( ErrorMessage = "กรุณาระบุเบอร์โทรศัพท์" )]
        public string Phone { get; set; }
        [DataType( DataType.EmailAddress )]
        [EmailAddress( ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง" )]
        public string Email { get; set; }

        public string Facebok { get; set; }
        [Required( ErrorMessage = "กรุณาระบุ Line ID" )]
        public string Line { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public DateTime CreateDate { get; set; }

        public List<WebSite> WebSites { get; set; }
        public List<Order> Orders { get; set; }

        public void SetCreatorName( string name ) { CreatorName = name; }
        public string GetCreatorName() { return CreatorName; }
        public void SetSaleName( string name ) { SaleName = name; }
        public string GetSaleName() { return SaleName; }
    }
}
