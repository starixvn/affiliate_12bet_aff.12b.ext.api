using Fanex.Aff12B.Common.Core.Constants;
using Fanex.Aff12B.Common.Core.Extensions;
using Fanex.Aff12B.Common.Core.Helpers;
using Fanex.Aff12B.Common.Core.Models;
using Fanex.Aff12B.Identity.Core.Services;

namespace Fanex.Aff12B.Identity.Host
{
    public class Startup(IConfiguration configuration)
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<List<User>>(configuration.GetSection(SettingKeys.Users));

            services.ConfigureServices(configuration, "Aff12B.Identity");

            DIRegister(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Configure(configuration);
        }

        private static void DIRegister(IServiceCollection services)
        {
            services.AddSingleton<IErrorMessageHelper, ErrorMessageHelper>();
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
        }
    }
}