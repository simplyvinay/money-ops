﻿using AspNetCoreRateLimit;
using MoneyOps.Infrastructure.Auth.Authentication;
using MoneyOps.Infrastructure.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyOps.Infrastructure
{
    public static class WebRegistry
    {
        public static IServiceCollection AddWeb(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //attach the the model validator and define the api grouping convention
            //setup fluent validation for the running assembly
            services.AddMvc(
                    options =>
                    {
                        options.Filters.Add<ValidateModelFilter>();
                        options.Conventions.Add(new GroupByApiRootConvention());
                    })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(
                implementationFactory =>
                {
                    var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                    return new UrlHelper(actionContext);
                });

            services.AddHttpCacheHeaders(
                expirationModelOptionsAction => { expirationModelOptionsAction.MaxAge = 120; },
                validationModelOptionsAction => { validationModelOptionsAction.MustRevalidate = true; });
            services.AddResponseCaching();

            services.AddMemoryCache();
            
            //Can be rate limited by Client Id as well
            //ClientRateLimitOptions
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddApiVersioning(
                config =>
                {
                    // Specify the default API Version
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    // If the client hasn't specified the API version in the request, use the default API version number 
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    // Advertise the API versions supported for the particular endpoint
                    config.ReportApiVersions = true;

                    // Supporting multiple versioning scheme
                    config.ApiVersionReader = ApiVersionReader.Combine(
                        new HeaderApiVersionReader("X-version"),
                        new QueryStringApiVersionReader("api-version"));
                });

            return services;
        }
    }
}