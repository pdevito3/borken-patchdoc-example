namespace RecipeManagement.Extensions.Services;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Serilog;
using Services;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(Log.Logger);
        // TODO update CORS for your env
        builder.Services.AddCorsService("RecipeManagementCorsPolicy", builder.Environment);
        builder.Services.OpenTelemetryRegistration("RecipeManagement");
        builder.Services.AddInfrastructure(builder.Environment);

        // using this will work
        // builder.Services.AddControllers()
        //     .AddNewtonsoftJson()
        //     .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        
        // using this will break
        builder.Services.AddControllers(options =>
        {
            options.InputFormatters.Insert(0, CustomNewtonsoftJsonPatchInputFormatter.GetJsonPatchInputFormatter());
        });
        
        builder.Services.AddApiVersioningExtension();
        builder.Services.AddWebApiServices();
        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension();
    }
}

public static class CustomNewtonsoftJsonPatchInputFormatter
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        var builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        return builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}