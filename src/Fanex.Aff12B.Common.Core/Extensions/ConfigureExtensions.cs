using Fanex.Aff12B.Common.Core.Constants;
using Fanex.Aff12B.Common.Core.Middlewares;
using Fanex.Aff12B.Common.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sentry.AspNetCore;

namespace Fanex.Aff12B.Common.Core.Extensions
{
    public static class ConfigureExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, string swaggerDocTitle, bool shouldConfigJwt = false)
        {
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerDocTitle, Version = "v1" });
            });

            services.Configure<SentryAspNetCoreOptions>(configuration.GetSection(SettingKeys.Sentry));
            services.Configure<TokenManagementOptions>(configuration.GetSection(SettingKeys.TokenManagement));

            if(shouldConfigJwt)
            {
                services.AddResponseCaching(options =>
                {
                    options.UseCaseSensitivePaths = false;
                });

                services.AddJwtAuthentication(configuration);

                services.AddAuthorizationBuilder()
                    .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            }

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

            services.AddUnderMaintenance(configuration);
        }

        public static void Configure(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UsePingCheck();
            app.UseForwardedHeaders();
            if (string.Equals(configuration["UseSwagger"], "1"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (string.Equals(configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseWhoAmICheck();
            app.UseUnderMaintenance();
        }
    }
}
