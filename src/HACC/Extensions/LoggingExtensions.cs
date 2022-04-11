using HACC.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace HACC.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddCustomLogging(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            descriptor: ServiceDescriptor.Singleton<ILoggerProvider, LoggingProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <LoggingConfiguration, LoggingProvider>(services: builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddCustomLogging(
        this ILoggingBuilder builder,
        Action<LoggingConfiguration> configure)
    {
        builder.AddCustomLogging();
        builder.Services.Configure(configureOptions: configure);

        return builder;
    }
}