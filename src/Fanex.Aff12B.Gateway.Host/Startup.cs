using Fanex.Aff12B.Common.Core.Constants;
using Fanex.Aff12B.Common.Core.Extensions;
using Fanex.Aff12B.Common.Core.Helpers;
using Fanex.Aff12B.Common.Core.Middlewares;
using Fanex.Aff12B.Common.Core.Models;
using Fanex.Aff12B.Gateway.Core.Attributes;
using Fanex.Aff12B.Gateway.Core.Handlers;
using Fanex.Aff12B.Gateway.Core.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Fanex.Aff12B.Gateway.Host
{
    public class Startup(IConfiguration configuration)
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationOptions>(configuration.GetSection(SettingKeys.ApplicationOptions));
            services.Configure<List<User>>(configuration.GetSection(SettingKeys.Users));

            services.ConfigureServices(configuration, "Aff12B.Gateway", true);

            services.AddOcelot(configuration).AddDelegatingHandler<ApiKeyHandler>(true);

            DIRegister(services);
        }

        public async Task Configure(IApplicationBuilder app)
        {
            app.Configure(configuration);

            app.UseMiddleware<RequestResponseLogging>();

            app.UseRouting();

            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseJWTokenOnPage();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
            });

            await app.UseOcelot();
            app.UseResponseCaching();
        }

        private static void DIRegister(IServiceCollection services)
        {
            services.AddSingleton<IErrorMessageHelper, ErrorMessageHelper>();
            services.AddScoped<IPFilterAttribute>();
        }
    }
}