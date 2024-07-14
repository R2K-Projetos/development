using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Ghb.Psicossoma.Crosscutting.IoC;
using Ghb.Psicossoma.Api.Configuration;
using Ghb.Psicossoma.Repositories.Context;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;

#if DEBUG
var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { EnvironmentName = "Debug" });
#elif HOMOL
var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { EnvironmentName = "homol" });
#endif


IConfigurationSection contextDatabaseSettings = builder.Configuration.GetSection(nameof(ContextDatabaseSettings));
string connectionString = contextDatabaseSettings.GetSection("ConnectionString").Value ?? "WithoutConnection";
string databaseName = contextDatabaseSettings.GetSection("Database").Value ?? "WithoutDatabase";

string environmentNameArg = builder.Environment.EnvironmentName;
string apiVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "0.0";
string apiName = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "ApiName";
string apiDescription = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "ApiDescription";

builder.Services
    .AddMemoryCache()
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.UseCamelCasing(false);
        options.SerializerSettings.Formatting = Formatting.None;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

builder.AddAuthentication();
builder.AddSwagger(apiVersion, apiName, apiDescription);
builder.AddLogger(connectionString, databaseName);
builder.Services.InitializeContainerIoC(contextDatabaseSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName.ToLower() == "homol"
    || app.Environment.EnvironmentName.ToLower() == "debug")
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", apiName);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

