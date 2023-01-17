using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Extensions;
using TakraonlineCRM.Server.Models;

namespace TakraonlineCRM.Server
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            var appBasePath = System.IO.Directory.GetCurrentDirectory();
            NLog.GlobalDiagnosticsContext.Set( "appbasepath", appBasePath );
            LogManager.LoadConfiguration( String.Concat( Directory.GetCurrentDirectory(), "/nlog.config" ) );
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddCors();
            services.ConfigureLoggerService();
            services.AddDbContext<ApplicationDBContext>( options => options.UseSqlServer( Configuration.GetConnectionString( "DefaultConnection" ) ) );
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
            services.ConfigureApplicationCookie( options =>
            {
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            } );

            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseStaticFiles( new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider( Path.Combine( Directory.GetCurrentDirectory(), @"StaticFiles" ) ),
                RequestPath = new PathString( "/StaticFiles" )
            } );

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints( endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile( "index.html" );
            } );
        }
    }
}
