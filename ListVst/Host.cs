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
            await CoconaApp
                .CreateHostBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    var config = builder.Build();
                    PrepareConfiguration(config);
                })
                .ConfigureServices(RegisterServices)
                .RunAsync<Program>();
        }

        private static void PrepareConfiguration(IConfiguration configuration)
        {
            var defaultSourcePath = configuration.GetValue<string>("defaultSourcePath");
            
            var listVstConfig = new Configuration
            {
                SourcePath = defaultSourcePath,
            };

            Configuration = listVstConfig;
        }
        
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            // TODO: Move to dynamic registration
            services.AddTransient<IProcessor, AbletonLive.Processor>();
            services.AddTransient<IProcessor, StudioOne.Processor>();

            var serviceProvider = services.BuildServiceProvider();
            var processors = serviceProvider.GetServices(typeof(IProcessor)).Cast<IProcessor>();
            Configuration.Processors = processors;
        }
    }
}
