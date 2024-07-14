using Serilog;
using System.Text;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog.Sinks;

namespace Ghb.Psicossoma.Api.Configuration
{
    public static class ApplicationConfiguration
    {
        public static void AddSwagger(this WebApplicationBuilder builder, string apiVersion, string apiName, string apiDescription)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{apiVersion}",
                    new OpenApiInfo
                    {
                        Title = apiName,
                        Version = $"v{apiVersion}",
                        Description = apiDescription
                    });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
                            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                options.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}_{apiDesc.HttpMethod}");

                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
            });
        }
        public static void AddLogger(this WebApplicationBuilder builder, string connectionString, string databaseName)
        {
            builder.Host.UseSerilog((context, configuration) => configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .WriteTo.Logger(config =>
                {
                    config.Filter.ByExcluding(Matching.WithProperty("DatabaseOnly"));
                    //config.Filter.ByIncludingOnly(x => x.Level >= LogEventLevel.Verbose);
                    config.WriteTo.Async(writeTo => {
                        writeTo.Console(
                            //restrictedToMinimumLevel: LogEventLevel.Verbose,
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Username} {Message:lj} {NewLine}{Exception}"
                        );
                    });
                })
                .WriteTo.Logger(config =>
                {
                    config.Filter.ByExcluding(Matching.WithProperty("ConsoleOnly"));
                    config.Filter.ByIncludingOnly(x => x.Level >= LogEventLevel.Warning);
                    config.WriteTo.Async(writeTo =>{ writeTo.MySQL(connectionString, "Logs"); });
                })
            );
        }

        public static void AddAuthentication(this WebApplicationBuilder builder)
        {
            var tokenKey = builder.Configuration.GetValue<string>("AuthenticationConfiguration:TokenKey");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = static context =>
                    {
                        if (context.Request.Query.TryGetValue("tk", out var token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
