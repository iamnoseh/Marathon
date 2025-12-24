using Application.Behaviors;
using FluentValidation;
using MediatR;

namespace WebApi.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.Responses.Response<>).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(Application.Responses.Response<>).Assembly);

        services.AddScoped<Application.Interfaces.IFileStorageService, Infrastructure.Services.FileStorageService>();

        return services;
    }
}
