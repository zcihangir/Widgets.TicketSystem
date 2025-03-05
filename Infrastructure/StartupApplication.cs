using Grand.Infrastructure;
using Grand.Business.Core.Interfaces.Cms;
using Grand.Business.Core.Interfaces.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Widgets.TicketSystem.Services;
using Grand.Web.Common.Menu;

namespace Widgets.TicketSystem.Infrastructure
{
    public class StartupApplication : IStartupApplication
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Widget provider
            services.AddScoped<IWidgetProvider, TicketSystemWidgetProvider>();
            
            // Servisler
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IContactFormService, ContactFormService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            
            // Admin menü provider
            services.AddScoped<IAdminMenuProvider, AdminMenuProvider>();
        }

        public int Priority => 10;
        
        public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
        {
            // Uygulama başlangıcında yapılacak konfigürasyonlar
        }
        
        public bool BeforeConfigure => false;
    }
} 