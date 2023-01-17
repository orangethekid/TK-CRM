using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TakraonlineCRM.Client.DataInterface;
using TakraonlineCRM.Client.Repository;
using TakraonlineCRM.Client.Services;
using Tewr.Blazor.FileReader;

namespace TakraonlineCRM.Client
{
    public class Program
    {
        public static async Task Main( string[] args )
        {
            var builder = WebAssemblyHostBuilder.CreateDefault( args );
            builder.RootComponents.Add<App>( "app" );

            builder.Services.AddOptions();
            builder.Services.AddLocalization();
            //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<CustomStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>( s => s.GetRequiredService<CustomStateProvider>() );
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IUploadRepository, UploadRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IDomainRepository, DomainRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IWebSiteRepository, WebSiteRepository>();
            builder.Services.AddScoped<IMarketingRepository, MarketingRepository>();
            builder.Services.AddScoped<IGraphicRepository, GraphicRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

            builder.Services.AddFileReaderService( o => o.UseWasmSharedBuffer = true );
            builder.Services.AddTransient( sp => new HttpClient { BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) } );

            await builder.Build().RunAsync();
        }
    }
}
