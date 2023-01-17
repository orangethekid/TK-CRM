using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TakraonlineCRM.Server.Models;
using TakraonlineCRM.Shared.Customers;
using TakraonlineCRM.Shared.Graphics;
using TakraonlineCRM.Shared.Marketing;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.Orders;
using TakraonlineCRM.Shared.Setting.Course;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Server.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext( DbContextOptions<ApplicationDBContext> options ) : base( options )
        {
        }
        //User
        public DbSet<ActivitiesLog> ActivitiesLogs { get; set; }
        //Customer
        public DbSet<Customer> Customers { get; set; }
        //Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderFinancial> OrderFinancials { get; set; }
        public DbSet<OrderWebSite> OrderWebSites { get; set; }
        public DbSet<OrderMarketing> OrderMarketings { get; set; }
        public DbSet<OrderCourse> OrderCourses { get; set; }
        public DbSet<OrderGraphic> OrderGraphics { get; set; }
        //Website
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<WebSiteNewDesign> WebSiteNewDesigns { get; set; }
        public DbSet<WebSiteProgramEdit> WebSiteProgramEdits { get; set; }
        public DbSet<Domain> Domains { get; set; }
        //Marketing
        public DbSet<FacebookAds> FabookAds { get; set; }
        public DbSet<LineAdsPlatform> LineAdsPlatforms { get; set; }
        public DbSet<GoogleShop> GoogleShops { get; set; }
        //Graphic
        public DbSet<Graphic> Graphics { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}

//-----------#1 Add New Migration----------
//run 'add-migration <T>'
//-----------#2 Update Database From latest Migration
//run 'update-database'
//-----------#3 Remove Latest Migration
//run 'remove-migration'
//-----------#4 Clean And Reset Migration With our Delete Table
//1.Delete the migrations folder in your project
//2.Delete the __MigrationHistory table in your database
//3.run 'Enable-Migrations -EnableAutomaticMigrations -Force'
//4.Add new Initial Migration
//5.Comment UP method in new migration
//6.Update

