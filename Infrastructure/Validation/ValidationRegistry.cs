using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyOps.Infrastructure.Validation
{
    public static class ValidationRegistry
    {
        //hook up validation into MediatR pipeline
        public static IServiceCollection AddValidationPipeline(
            this IServiceCollection services)
        {
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationPipelineBehavior<,>));
            return services;
        }
    }
}