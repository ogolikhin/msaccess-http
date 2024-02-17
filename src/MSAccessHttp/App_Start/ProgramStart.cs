using MSAccessHttp.ExceptionHandling;
using System.Text.Json.Serialization;

namespace MSAccessHttp.App_Start;

public static class ProgramStart
{
    public static void AddServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    public static void RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseExceptionHandler();

        app.MapControllers();

        app.Run();
    }
}
