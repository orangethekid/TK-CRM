using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.WebSites
{
    public class WebSite : BaseEntity
    {
        public WebSite()
        {
            CreateDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(1);
            Domains = new List<Domain>();
            NewDesign = new List<WebSiteNewDesign>();
            ProgramEdit = new List<WebSiteProgramEdit>();
        }

        public int CustomerId { get; set; }
        public int OrderId { get; set; }

        [Required( ErrorMessage = "กรุณาระบุชื่อเว็บไซต์" )]
        public string Name { get; set; }
        public string Url { get;set;}

        public string Version { get; set; }
        [Required( ErrorMessage = "กรุณาระบุจำนวน สินค้า/Sale Page สูงสุด" )]
        public int MaximumProduct { get; set; }
        public string Package { get; set; }
        public string Detail { get; set; }
        public bool IsInstallTemplate { get; set; } = false;
        public string TemplateName { get; set; }
        public bool IsNewDesign { get; set; } = false;
        public bool IsProgramEdit { get; set; } = false;       
        public bool IsFree { get; set; } = false;
        public bool IsCompanyProfile { get; set; } = false;
        public bool IsSalePlage { get; set; } = false;
        public bool IsFacebookShop { get; set; } = false;
        public bool IsGoogleMarket { get; set; } = false;
        public bool IsLazada { get; set; } = false;
        public bool IsShopee { get; set; } = false;

        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Domain> Domains { get; set; }
        public List<WebSiteNewDesign> NewDesign { get; set; }
        public List<WebSiteProgramEdit> ProgramEdit { get; set; }
        public string GetWebSiteType()
        {
            if (IsFree) { return "ร้านค้าฟรี"; }
            else if (IsCompanyProfile) { return "เว็บไซต์ข้อมูลบริษัท"; }
            else if (IsSalePlage) { return "Sale Page"; }
            else { return "ร้านค้าชั้นธุรกิจ"; }
        }
    }

    public class WebSiteNewDesign : BaseEntity
    {
        public int WebsiteID { get; set; }
        public int OrderId { get; set; }
        public int DraftVersion { get; set; }
        public string Detail { get; set; }
        public bool IsApprove { get; set; }
        public DateTime ApproveDate { get; set; }
        public string DesignPath { get; set; }
    }

    public class WebSiteProgramEdit : BaseEntity
    {
        public int WebSiteID { get; set; }
        public int OrderId { get; set; }
        public int EstimateVersion { get; set; }
        public string Detail { get; set; }
        public bool IsApprove { get; set; }
        public DateTime AppoveDate { get; set; }
        public string EstimatePath { get; set; }
        public string MannualPath { get; set; }
    }
}
