using Fanex.Aff12B.Ext.Api.Core.Middleware;
using Fanex.Aff12B.Ext.Api.Core.Models;
using Fanex.Data.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Fanex.Aff12B.Ext.Api.Core.Services;
using Fanex.Aff12B.Ext.Api.Core.Extensions;
using Fanex.Aff12B.Ext.Api.Core.Attributes;
using Sentry.AspNetCore;
using Fanex.Aff12B.Ext.Api.Core.Utilities;

namespace Fanex.Aff12B.Ext.Api.Host
{
    public class Startup
    {
        private const string ApplicationOptionSettingKey = "ApplicationOptions";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<SentryAspNetCoreOptions>(Configuration.GetSection("Sentry"));
            services.Configure<TokenManagementOptions>(Configuration.GetSection("TokenManagement"));
            services.Configure<ApplicationOptions>(Configuration.GetSection(ApplicationOptionSettingKey));

            services.Configure<List<User>>(Configuration.GetSection("Users"));

            services.AddFanexSql(Configuration);
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });

            ConfigAuthentication(services);

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddHealthChecks()
                .AddCheck<WhoAmIHealthCheck>("WhoAmIHealthCheck");

            services.AddHttpContextAccessor();
            services.AddHttpsRedirection(options =>
            {
                const int httpsPort = 443;
                options.HttpsPort = httpsPort;
            });

            DIRegister(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePingCheck();
            app.UseForwardedHeaders();
            if (string.Equals(Configuration["UseSwagger"], "1"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWhoAmICheck();
            if (!env.IsDevelopment())
            {    
                app.UseHsts();

                app.UseWhen(
                    context => !context.Request.FromInternal(),
                    builder =>
                    {
                        builder.UseHttpsRedirection();
                    });
            }

            app.UseStaticResource(env.IsDevelopment());
            app.UseUnderMaintenance();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseJWTokenOnPage();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();
        }

        private void DIRegister(IServiceCollection services)
        {
            services.AddScoped<IAffService, AffService>();
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IPFilterAttribute>();

            services.AddUnderMaintenance(Configuration);
        }

        private void ConfigAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = TokenUtils.GetTokenValidationParameters(Configuration);
                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Append("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }                        
                    };
                });
        }
    }
}