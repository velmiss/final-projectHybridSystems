using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(builder.Configuration.GetSection("NoviaHybrid:ApiScopes")
    .Get<string>().Split(" ", System.StringSplitOptions.RemoveEmptyEntries))
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;

    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
    options.AddPolicy("RequireContributorRole", policy => policy.RequireRole("contributor", "admin"));
    options.AddPolicy("RequireMemberRole", policy => policy.RequireRole("contributor", "admin", "member"));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
var app = builder.Build();

ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);

GraphServiceClient graphServiceClient = new GraphServiceClient(authenticationProvider);
var result = graphServiceClient.Users.Request().GetAsync();
foreach (var item in result.Result)
{
    Console.WriteLine(item.DisplayName);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //use another appsettings.json file for production
    
    

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
