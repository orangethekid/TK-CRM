using System;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.Orders
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDate = DateTime.Now;
            CreateDate = DateTime.Now;
            TransferDate = DateTime.Now;
            Financial = new OrderFinancial();
            Website = new OrderWebSite();
            Marketing = new OrderMarketing();
            Graphic = new OrderGraphic();
            Course = new OrderCourse();
        }

        public int CustomerId { get; set; }
        public string CreatorId { get; set; }
        private string CreatorName { get; set; }
        [Required( ErrorMessage = "กรุณาระบุหมายเลขใบสั่งซื้อ" )]
        public string TakraOrderId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusDetail { get; set; }

        public bool IsRead { get; set; } = false;

        //Order Type
        [Required( ErrorMessage = "กรุณาระบุประเภทบริการ" )]
        public string OrderType { get; set; }
        public OrderWebSite Website { get; set; }
        public OrderMarketing Marketing { get; set; }
        public OrderCourse Course { get; set; }
        public OrderGraphic Graphic { get; set; }

        //Order Financial
        public string Promotion { get; set; }
        public string Note { get; set; }
        public OrderFinancial Financial { get; set; }
        public string TransferReceipt { get; set; }
        public string TransferDetail { get; set; }
        public DateTime TransferDate { get; set; }

        public void SetCreatorName( string name ) { CreatorName = name; }
        public string GetCreatorName() { return CreatorName; }
    }

    public class OrderFinancial : BaseEntity
    {
        public int OrderId { get; set; }
        [Required( ErrorMessage = "กรุณาระบุราคา" )]
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Vat { get; set; }
        public double SubTotal { get; set; }
    }

    public class OrderWebSite : BaseEntity
    {
        public int OrderId { get; set; }
        //WebSite Package
        public bool Website { get; set; }
        public bool Upgrade { get; set; }
        public bool Plugins { get; set; }
        public int WebSiteId { get; set; }
        //Domain
        public bool Domain { get; set; }
        public bool SSL { get; set; }
        public int DominaId { get; set; }
        //Design
        public bool NewDesign { get; set; }
        public int NewDesignId { get; set; }
        //ProgramEdit
        public bool ProgramEdit { get; set; }
        public int ProgramEdiId { get; set; }
    }

    public class OrderMarketing : BaseEntity
    {
        public int OrderId { get; set; }
        //Marketing Pakage
        public bool Facebook { get; set; }
        public bool LineAdsPlatform { get; set; }
        public bool GoogleShop { get; set; }
        public string Detail { get; set; }
    }

    public class OrderCourse : BaseEntity
    {
        public int OrderId { get; set; }
        //Course
        public string CourseName { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }

    public class OrderGraphic : BaseEntity
    {
        public int OrderId { get; set; }
        //Detail
        public string Purpose { get; set; }
        public string FocusDetail { get; set; }
        public DateTime DraftDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
