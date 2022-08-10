using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Common.CommonModels.Settings;
using PostReader.Api.Common.Interfaces;
using PostReader.Api.Common.Middleware;
using PostReader.Api.Infrastructure.Services;
using PostReader.Api.Services;
using PostReader.Api.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var services = builder.Services;

services.AddControllersWithViews();
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining<GetPostsQueryValidator>();

services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddAutoMapper(Assembly.GetExecutingAssembly());

var europePmcSettings = new EuropePmcSettings();
configuration.GetSection("EuropePmcSettings").Bind(europePmcSettings);
services.AddSingleton(europePmcSettings);

services.AddScoped<DeveloperErrorHandlingMiddleware>();
services.AddScoped<ErrorHandlingMiddleware>();
services.AddScoped<RequestTimeMiddleware>();
services.AddTransient<IWebsitesReaderService, WebsiteReaderService>();
services.AddTransient<IRequestWebsiteService, RequestWebsiteServices>();
services.AddSingleton<IPaginationListService, PaginationListService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<DeveloperErrorHandlingMiddleware>();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseMiddleware<RequestTimeMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
