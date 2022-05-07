namespace RecipeManagement.Extensions.Services;

using System.Text.Json.Serialization;
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

        // using Newtonsoft.Json to support PATCH docs since System.Text.Json does not support them https://github.com/dotnet/aspnetcore/issues/24333
        // if you are not using PatchDocs and would prefer to use System.Text.Json, you can remove The `AddNewtonSoftJson()` line
        builder.Services.AddControllers()
            .AddNewtonsoftJson()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddApiVersioningExtension();
        builder.Services.AddWebApiServices();
        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension();
    }
}