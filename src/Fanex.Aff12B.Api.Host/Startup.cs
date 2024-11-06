using Fanex.Aff12B.Api.Core.Attributes;
using Fanex.Aff12B.Api.Core.Helpers;
using Fanex.Aff12B.Api.Core.Services;
using Fanex.Aff12B.Common.Core.Extensions;
using Fanex.Aff12B.Common.Core.Helpers;
using Fanex.Aff12B.Common.Core.Middlewares;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

namespace Fanex.Aff12B.Api.Host
{
    public class Startup(IConfiguration configuration)
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFanexSql(configuration);

            services.ConfigureServices(configuration, "Aff12B.Api", true);

            DIRegister(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Configure(configuration);

            app.UseJWTokenOnPage();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();
        }

        private static void DIRegister(IServiceCollection services)
        {
            services.AddSingleton<IErrorMessageHelper, ErrorDbMessageHelper>();
            services.AddScoped<IDynamicRepository, DynamicRepository>();

            services.AddScoped<IAffService, AffService>();
            services.AddScoped<AffFilterAttribute>();
        }
    }
}