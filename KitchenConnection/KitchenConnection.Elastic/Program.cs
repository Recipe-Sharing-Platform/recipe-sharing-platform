using Elasticsearch.Net;
using KitchenConnection.Models.HelperModels;
using KitchenConnection.Elastic.BackgroundServices;
using KitchenConnection.Elastic.MessageHandlers;
using KitchenConnection.Elastic.Models;
using KitchenConnection.Elastic.Services;
using KitchenConnection.Elastic.Services.IServices;
using Nest;
using KitchenConnection.Models.Dispatcher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add the elastic connection
var pool = new SingleNodeConnectionPool(new Uri(builder.Configuration.GetConnectionString("elasticSearch")!));
var settings = new ConnectionSettings(pool).DefaultIndex("recipe");
var client = new ElasticClient(settings);
builder.Services.AddSingleton((IElasticClient)client);

// add the message handlers
builder.Services.AddTransient<IMessageHandler<IndexRecipe>, IndexRecipeMessageHandler>();
builder.Services.AddTransient<IMessageHandler<DeleteRecipe>, DeleteRecipeMessageHandler>();
builder.Services.AddTransient<IMessageHandler<UpdateRecipe>, UpdateRecipeMessageHandler>();

RabbitMqConfig rabbitMqConfig = builder.Configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>()!;
builder.Services.AddSingleton(rabbitMqConfig);

builder.Services.AddHostedService<Consumer>(); // add the RabbitMQ Consumer as a background service

builder.Services.AddScoped<ISearchService ,SearchService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();