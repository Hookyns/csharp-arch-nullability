using Arch.Nullability.Program.Infrastructure.Configurators;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Arch.Nullability.Program.Configurators;

public class SwaggerConfigurator : IConfigurator
{
    private static readonly string ApiName = "ArchDemo";
    private IReadOnlyList<ApiVersionDescription>? _apiVersionsDescription;

    public void Configure(WebApplicationBuilder builder)
    {
        builder.Services
            .AddSwaggerGen(options =>
            {
                foreach (ApiVersionDescription description in _apiVersionsDescription ?? throw new InvalidOperationException())
                {
                    options.SwaggerDoc($"{description.GroupName}v{description.ApiVersion}", CreateVersionInfo(description));
                }

                // https://stackoverflow.com/a/58972781/7141090;
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                            Template: 'Bearer {token}'
                            Example: 'Bearer eyJhbG.eyJJU3MjF9.u7_2W5SP-Z4xBm9yKNZ-Bi06Q'
                            Use /api/auth/login endpoint to get the Bearer token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                // To show locks on endpoints in the UI. Currently it works even without this.
                // options.OperationFilter<SecurityRequirementsOperationFilter>();

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                TryAddXmlComments(options, typeof(SwaggerConfigurator).Assembly.GetName().Name + ".xml");
            });
    }

    public void Configure(WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                _apiVersionsDescription = app.DescribeApiVersions();

                foreach (ApiVersionDescription description in _apiVersionsDescription)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/{description.ApiVersion}/swagger.json",
                        $"{ApiName} - {description.GroupName.ToUpperInvariant()}"
                    );
                }
            })
            ;
    }

    /// <summary>
    /// Tries to add generated XML comments into the swagger.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assemblyXmlFileName"></param>
    private void TryAddXmlComments(SwaggerGenOptions options, string assemblyXmlFileName)
    {
        try
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, assemblyXmlFileName));
        }
        catch (Exception)
        {
            // serviceProvider?.GetService<ILogger<SwaggerConfigurator>>()?.LogWarning($"File '{assemblyXmlFileName}' with assembly XML comments not found.");
            // forget...
        }
    }

    /// <summary>
    /// Create information about the version of the API
    /// </summary>
    /// <param name="description"></param>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = $"{ApiName} {description.GroupName} API v{description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description = "This API version has been deprecated." + info.Description;
        }

        return info;
    }
}