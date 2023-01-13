using KitchenConnection.DataLayer.Data;
using KitchenConnection.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<KitchenConnectionDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), serverOptions => serverOptions.MigrationsAssembly("KitchenConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
              .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
              {
                  options.Authority = "https://sso-sts.gjirafa.dev";
                  options.RequireHttpsMetadata = false;
                  options.Audience = "api_scope";
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "https://sso-sts.gjirafa.dev",
                      ValidAudience = "rsp_api",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("e9872cad-4992-46f6-9de7-908b796387be")),
                      ClockSkew = TimeSpan.Zero
                  };

                  // if token does not contain a dot, it is a reference token
                  options.ForwardDefaultSelector = Selector.ForwardReferenceToken("token");
              });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Life Ecommerce", Version = "v1" });
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();