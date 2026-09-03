using Microsoft.AspNetCore.HttpOverrides;
using TestApp.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedPrefix;

    // LOCAL TEST ONLY. Do not copy this trust policy to production.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    app.Logger.LogInformation(
        "BEFORE: PrefixHeader={PrefixHeader}; PathBase={PathBase}; Path={Path}",
        context.Request.Headers["X-Forwarded-Prefix"].ToString(),
        context.Request.PathBase,
        context.Request.Path);
    await next();
});

app.UseForwardedHeaders();

app.Use(async (context, next) =>
{
    app.Logger.LogInformation(
        "AFTER: PrefixHeader={PrefixHeader}; PathBase={PathBase}; Path={Path}",
        context.Request.Headers["X-Forwarded-Prefix"].ToString(),
        context.Request.PathBase,
        context.Request.Path);
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// For this HTTP-only local test, disable HTTPS redirection if it redirects
// the proxy traffic to an endpoint that the harness doesn't expose.
// app.UseHttpsRedirection();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();