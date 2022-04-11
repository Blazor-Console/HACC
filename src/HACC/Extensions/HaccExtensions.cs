using HACC.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HACC.Extensions;

public static class HaccExtensions
{
    private const string DefaultError = "Call UseHacc() first";
    private static ServiceProvider? _serviceProvider;
    private static ILoggerFactory? _loggerFactory;

    private static WebClipboard? _webClipboard;

    public static WebClipboard WebClipboard =>
        _webClipboard ?? throw new InvalidOperationException(message: DefaultError);

    public static ServiceProvider ServiceProvider =>
        _serviceProvider ?? throw new InvalidOperationException(message: DefaultError);

    public static ILoggerFactory LoggerFactory =>
        _loggerFactory ?? throw new InvalidOperationException(message: DefaultError);

    public static ILogger<T> CreateLogger<T>()
    {
        return LoggerFactory.CreateLogger<T>();
    }

    public static WebAssemblyHostBuilder UseHacc(this WebAssemblyHostBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddCustomLogging(configure: configuration =>
        {
            configuration.LogLevels.Add(
                key: LogLevel.Warning,
                value: ConsoleColor.DarkMagenta);
            configuration.LogLevels.Add(
                key: LogLevel.Error,
                value: ConsoleColor.Red);
        });
        builder.Logging.SetMinimumLevel(level: LogLevel.Debug);

        _serviceProvider = builder.Services.BuildServiceProvider();
        _loggerFactory = _serviceProvider.GetService<ILoggerFactory>()!;
        _webClipboard = new WebClipboard();

        return builder;
    }

    public static T GetService<T>()
    {
        return ServiceProvider.GetService<T>()!;
    }
}