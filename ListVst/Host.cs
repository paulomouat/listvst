using System;
using System.Configuration;
using System.Threading.Tasks;
using Cocona;
using ListVst.OutputFormatting;
using ListVst.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ListVst;

internal class Host
{
    private static IConfiguration? Configuration { get; set; }
    
    static async Task Main(string[] args)
    {
        var builder = CoconaApp
            .CreateHostBuilder()
            .ConfigureLogging((ctx, builder) =>
            {
                AddBareConsoleFormatterIfConfigured(ctx, builder);
            })
            .ConfigureServices((ctx, services) =>
            {
                Configuration = ctx.Configuration;
                RegisterServices(services);
            });
        
        await builder.RunAsync<Program>(args, options =>
            {
                options.TreatPublicMethodsAsCommands = false;
            });

        await Task.CompletedTask;
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
        RegisterProcessors(services);
        RegisterOutputFormatters(services);
    }

    private static void RegisterProcessors(IServiceCollection services)
    {
        //services.Configure<ProcessingConfiguration>(section);

        var configuration = new ProcessingConfiguration();
        var section = Configuration?.GetSection(ProcessingConfiguration.SectionName);
        section.Bind(configuration);
        
        foreach (var processorTypeName in configuration.Processors)
        {
            var processorType = Type.GetType(processorTypeName);
            if (processorType is null)
            {
                throw new ConfigurationErrorsException(
                    $"The processor {processorTypeName} indicated in configuration was not found.");
            }

            if (!processorType.IsAssignableTo(typeof(IProcessor)))
            {
                throw new ConfigurationErrorsException(
                    $"The processor {processorTypeName} indicated in configuration must implement IProcessor.");
            }

            services.AddSingleton(typeof(IProcessor), processorType);
        }

        services.AddSingleton<IProcessorRegistry, ProcessorRegistry>();
    }

    private static void RegisterOutputFormatters(IServiceCollection services)
    {
        //services.Configure<OutputFormattingConfiguration>(section);

        var configuration = new OutputFormattingConfiguration();
        var section = Configuration?.GetSection(OutputFormattingConfiguration.SectionName);
        section.Bind(configuration);

        foreach (var formatterTypeName in configuration.Formatters)
        {
            var formatterType = Type.GetType(formatterTypeName);
            if (formatterType is null)
            {
                throw new ConfigurationErrorsException(
                    $"The output formatter {formatterTypeName} indicated in configuration was not found.");
            }

            if (!formatterType.IsAssignableTo(typeof(IOutputFormatter)))
            {
                throw new ConfigurationErrorsException(
                    $"The output formatter {formatterTypeName} indicated in configuration must implement IOutputFormatter.");
            }

            services.AddSingleton(typeof(IOutputFormatter), formatterType);
        }

        services.AddSingleton<IOutputFormatterRegistry, OutputFormatterRegistry>();
    }
}
