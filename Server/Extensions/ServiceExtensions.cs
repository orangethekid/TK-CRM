using Microsoft.Extensions.DependencyInjection;
using TakraonlineCRM.Server.Contracts;
using TakraonlineCRM.Server.LoggerService;

namespace TakraonlineCRM.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService( this IServiceCollection services )
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
