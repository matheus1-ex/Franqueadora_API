using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using Franqueada.API.Data;
using Franqueada.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Franqueada.API.Services;

namespace  Franqueada.API;
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
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        // 2. Configuração do Swagger compatível com Swashbuckle v10 / OpenAPI v2
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1.0", new OpenApiInfo 
            { 
                Title = "Franqueada.API", 
                Version = "v1.0" 
            });

            // Instancia o esquema de segurança
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT gerado no endpoint de login."
            };

            // Registra a definição no Swagger
            c.AddSecurityDefinition("Bearer", securityScheme);

            // Passa a instância 'securityScheme' diretamente como chave (SEM usar .Reference)
            var securityRequirement = new OpenApiSecurityRequirement();
            securityRequirement[new OpenApiSecuritySchemeReference("Bearer")] = new List<string>();

            c.AddSecurityRequirement(SHA3_256 => securityRequirement);
        });

        // 3. Autenticação e Autorização JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuaChaveSuperSecretaParaDev12345!"))
                };
            });

        builder.Services.AddAuthorization();

        // Serviços do Sistema 
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();  
        builder.Services.AddScoped<IEstoqueService, EstoqueService>();
        builder.Services.AddScoped<IFinanceiroService, FinanceiroService>();
        builder.Services.AddScoped<IFornecedorService, FornecedorService>();
        builder.Services.AddScoped<IRelatorioService, RelatorioService>();
        builder.Services.AddScoped<IUnidadeService, UnidadeService>();  
        builder.Services.AddScoped<IVendaService, VendaService>();

        var app = builder.Build();

        app.UseMiddleware<RequestLogginMiddleWare>();
        app.MapGet("/", () => Results.Redirect("/swagger"));

        Console.WriteLine($"---> O SQLite está gravando em: {System.IO.Path.GetFullPath("franqueadora.db")}");


        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Para SQLite em desenvolvimento, cria o banco/tabelas caso não existam
            dbContext.Database.EnsureCreated();
        }

        // 4. Middlewares
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Franqueada.API v1.0");
                c.RoutePrefix = "swagger";
            });
        }
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}