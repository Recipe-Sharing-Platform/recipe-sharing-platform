using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Models.HelperModels;
using KitchenConnection.Notifier;
using KitchenConnection.Notifier.MessageHandlers;
using KitchenConnection.Notifier.Models;
using KitchenConnection.Notifier.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

// var configuration = new ConfigurationBuilder()
//      .AddJsonFile($"appsettings.json");
// var config = configuration.Build();

// var config = hostBuilder.Build().Services.GetRequiredService<IConfiguration>();

// var smtpConfigurations = config.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>()!;
// var rabbitMqConfig = config.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>()!;


var serilog = new LoggerConfiguration()
    // write to console
    .WriteTo.Console()
    // write to file
    .Enrich.FromLogContext()
    .CreateLogger();

var host = hostBuilder
    .ConfigureServices(services => {
        // get configuration
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var smtpConfigurations = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>()!;
        var rabbitMqConfig = configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>()!;

        services.AddSingleton(smtpConfigurations!);
        services.AddSingleton(rabbitMqConfig!);
        services.AddLogging(loggingBuilder => {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(serilog);
          }
        );
        services.AddTransient<IMessageHandler<CreatedReview>, CreatedReviewMessageHandler>();
        services.AddTransient<IMessageHandler<CreatedRecipe>, CreatedRecipeMessageHandler>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();
        services.AddHostedService<Worker>();
    })
    .Build();


host.Run();
