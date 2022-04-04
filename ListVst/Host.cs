using System.Linq;
using System.Threading.Tasks;
using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ListVst;

internal class Host
{
    private static Configuration Configuration { get; set; } = new();
    
    static async Task Main(string[] args)
    {
        var builder = CoconaApp
            .CreateHostBuilder()
            .ConfigureLogging((ctx, builder) =>
            {
                AddBareConsoleFormatterIfConfigured(ctx, builder);
            })
            .ConfigureServices(RegisterServices);
        
        await builder.RunAsync<Program>(args, options =>
            {
                options.TreatPublicMethodsAsCommands = false;
            });
    }

    private static void AddBareConsoleFormatterIfConfigured(HostBuilderContext ctx, ILoggingBuilder builder)
    {
        var formatterName = ctx.Configuration
            .GetSection("Logging")
            .GetSection("Console")
            .GetValue<string>("FormatterName");
        if (formatterName?.ToLowerInvariant() == "bare")
        {
            builder
                .AddConsole(options => options.FormatterName = "bare")
                .AddConsoleFormatter<BareConsoleFormatter, ConsoleFormatterOptions>();
        }
    }
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);

        RegisterProcessors(services);
        RegisterOutputFormatters(services);
    }

    private static void RegisterProcessors(IServiceCollection services)
    {
        // TODO: Move to dynamic registration
        services.AddTransient<IProcessor, AbletonLive.Processor>();
        services.AddTransient<IProcessor, StudioOne.Processor>();

        var serviceProvider = services.BuildServiceProvider();
        var processors = serviceProvider.GetServices(typeof(IProcessor)).Cast<IProcessor>();
        Configuration.Processors = processors;
    }

    private static void RegisterOutputFormatters(IServiceCollection services)
    {
        // TODO: Move to dynamic registration
        services.AddTransient<IOutputFormatter, OutputFormatters.TxtFile>();

        var serviceProvider = services.BuildServiceProvider();
        var formatters = serviceProvider.GetServices(typeof(IOutputFormatter)).Cast<IOutputFormatter>();

        var registry = new OutputFormatterRegistry();
        foreach (var formatter in formatters)
        {
            registry.Register(formatter.Format, formatter);
        }

        services.AddSingleton<IOutputFormatterRegistry>(sp => registry);
    }
}
