﻿using PostReader.Api.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostReader.Api.Common.Middleware
{
    public class DeveloperErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<DeveloperErrorHandlingMiddleware> _logger;

        public DeveloperErrorHandlingMiddleware(ILogger<DeveloperErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException badRequest)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequest.Message);
            }
            catch(NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch(Exception exceptions)
            {
                _logger.LogError(exceptions, exceptions.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(exceptions.Message);
            }
        }
    }
}
