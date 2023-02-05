using AutoMapper;
using FluentValidation.AspNetCore;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Helpers.FluentValidationMiddleware;
using KitchenConnection.DataLayer.Data;
using KitchenConnection.DataLayer.Hubs;
using KitchenConnection.Helpers;
using KitchenConnection.Models.HelperModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<FluentValidationFilter>();
    }
).AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params
        x.JsonSerializerOptions.IgnoreNullValues = true;
    }
).AddFluentValidation(fv => { });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<KitchenConnectionDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// run migrations on startup
//builder.Services.BuildServiceProvider().GetService<KitchenConnectionDbContext>().Database.Migrate();


var mapperConfiguration = new MapperConfiguration(
    mc => mc.AddProfile(new AutoMapperConfigurations()));

IMapper mapper = mapperConfiguration.CreateMapper();

builder.Services.AddSingleton(mapper);

// Add Services
builder.Services.AddServices();

// Add Validators
builder.Services.AddValidators();

// Add Serilog
var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

RabbitMqConfig rabbitMqConfig = builder.Configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>()!;
builder.Services.AddSingleton(rabbitMqConfig);
builder.Services.AddSingleton<MessageSender>();


// Add Porta
builder.Services.AddPorta();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kitchen Connection - Recipe Sharing Platform", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://sso-sts.gjirafa.dev/connect/authorize"),
                TokenUrl = new Uri("https://sso-sts.gjirafa.dev/connect/token"),
                Scopes = new Dictionary<string, string> {
                                              { "rsp_api", "Recipe Sharing Platform - API Scope" }
                                          }
            }
        }
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();
});
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DisplayRequestDuration();
    c.DefaultModelExpandDepth(0);
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KitchenConnection");
    c.OAuthClientId("9b6b8081-d9bf-4b3d-ba76-dd01132330bf");
    c.OAuthClientSecret("e9872cad-4992-46f6-9de7-908b796387be");
    c.OAuthAppName("Recipe Sharing Platform");
    c.OAuthUsePkce();
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

// run migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<KitchenConnectionDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

// Add Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.MapHub<NotificationHub>("/signalhub");

app.Run();