using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using Franqueada.API.Data;
using Franqueada.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Franqueada.API.Services;

namespace Franqueada.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=franqueadora.db";

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        // 1. Controllers + Serialização de Enums como String no JSON
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // 2. Configuração do Swagger com esquema Http Bearer (O Swagger coloca o "Bearer " automaticamente)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Franqueada.API", Version = "v1.0" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http, // Alterado de ApiKey para Http
                Scheme = "bearer",              // Em minúsculo para o Swagger formatar o Bearer
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira APENAS o seu token JWT abaixo (não precisa digitar a palavra 'Bearer ')."
            });

            c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        });

        // 3. Autenticação e Autorização JWT
        var jwtKey = builder.Configuration["Jwt:Key"] 
            ?? builder.Configuration["Jwt:SecretKey"] 
            ?? "k9X$mP2!vL7QnR4#T8zY1xU5cB3vA6mK";

        builder.Services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        { 
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "Franqueadora_API",

                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "Franqueadora.API",

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("--> FALHA NO JWT: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine("--> REQUISIÇÃO SEM TOKEN OU CABEÇALHO INVÁLIDO");
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.AddAuthorization();

        // 4. Injeção de Dependência dos Serviços
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();  
        builder.Services.AddScoped<IEstoqueService, EstoqueService>();
        builder.Services.AddScoped<IFinanceiroService, FinanceiroService>();
        builder.Services.AddScoped<IFornecedorService, FornecedorService>();
        builder.Services.AddScoped<IRelatorioService, RelatorioService>();
        builder.Services.AddScoped<IUnidadeService, UnidadeService>();  
        builder.Services.AddScoped<IVendaService, VendaService>();
        builder.Services.AddScoped<IFranquiaService, FranquiaService>();
        builder.Services.AddScoped<IFranqueadoraService, FranqueadoraService>();
        
        // Registrado com a interface ITokenService
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<ITokenService>(sp => sp.GetRequiredService<TokenService>());
        
        var app = builder.Build();

        Console.WriteLine($"---> O SQLite está gravando em: {System.IO.Path.GetFullPath("franqueadora.db")}");

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }

        // 5. Middlewares
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Franqueada.API v1.0");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseMiddleware<RequestLogginMiddleWare>();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => Results.Redirect("/swagger"));
        app.MapControllers();

        app.Run();
    }
}