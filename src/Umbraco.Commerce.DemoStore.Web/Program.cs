using Flurl.Http;
using Umbraco.Commerce.DemoStore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddDemoStore()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();

 app.Use(async (context, next) =>
 {
     context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
     context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
     await next();
 });

FlurlHttp.Clients.WithDefaults(cfg => cfg.OnError(async (req) =>
{
    // When error happens, try to log the response body if possible.
    try
    {
        var logger = app.Services.GetRequiredService<ILogger<IFlurlRequest>>();
        var responseBody = await req.Response.GetStringAsync();
        logger.LogError("Http request failed. Response body: \"{responseBody}\"", responseBody);
    }
    catch
    {
        // Ignore any error when logging.
    }
}));

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });
    
await app.RunAsync();
