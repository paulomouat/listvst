using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ListVst
{
    class Host
    {
        private static Configuration Configuration { get; set; } = new();
        
        static async Task Main(string[] args)
        {
            var builder = CoconaApp
                .CreateHostBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    var config = builder.Build();
                    PrepareConfiguration(config);
                })
                .ConfigureServices(RegisterServices);
            
            await builder.RunAsync<Program>(args, options =>
                {
                    options.TreatPublicMethodsAsCommands = false;
                });
        }

        private static void PrepareConfiguration(IConfiguration configuration)
        {
            var defaultSourcePath = configuration.GetValue<string>("defaultSourcePath");
            var outputs = configuration.GetSection("outputs").Get<IEnumerable<OutputDetails>>();
            
            var listVstConfig = new Configuration
            {
                SourcePath = defaultSourcePath,
                Outputs = outputs
            };

            Configuration = listVstConfig;
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
}
