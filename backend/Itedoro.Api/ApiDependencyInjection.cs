using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace Itedoro.Api;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddOperationTransformer((document, context, cancellationToken) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                var hasAllowAnonymous = metadata.OfType<IAllowAnonymous>().Any();
                var hasAuthorize = metadata.OfType<IAuthorizeData>().Any();

                if (hasAllowAnonymous || !hasAuthorize)
                {
                    return Task.CompletedTask;
                }

                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", context.Document)] = []
                });
                return Task.CompletedTask;
            });

            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme."
                    }
                };
                return Task.CompletedTask;
            });
        });
        return services;
    }
}