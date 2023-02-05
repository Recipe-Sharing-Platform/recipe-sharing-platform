using KitchenConnection.Models.HelperModels;
using KitchenConnection.Notifier;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();
var smtpConfigurations = config.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();


var host = hostBuilder
    .ConfigureServices(services => {
        services.AddSingleton(smtpConfigurations!);
        services.AddHostedService<Worker>();
    })
    .Build();


host.Run();
