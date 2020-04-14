﻿using Microsoft.AspNetCore.Builder;

namespace MoneyOps.Infrastructure.ErrorHandling
{
    public static class ErrorHandlingService
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}