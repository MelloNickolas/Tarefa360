using Projeto360.Repository;
using Projeto360.Repository.Interfaces;
using Projeto360.Application.Interfaces;
using Projeto360.Application;
using Projeto360.Services.Interfaces;
using Projeto360.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddScoped<IUsuarioApplication, UsuarioApplication>();
builder.Services.AddScoped<ITarefaApplication, TarefaApplication>();
builder.Services.AddScoped<IProjetoApplication, ProjetoApplication>();
builder.Services.AddScoped<IHistoriaApplication, HistoriaApplication>();
builder.Services.AddScoped<ISprintApplication, SprintApplication>();
builder.Services.AddScoped<IEquipeApplication, EquipeApplication>();
builder.Services.AddScoped<IUsuarioEquipeApplication, UsuarioEquipeApplication>();
builder.Services.AddScoped<IAutenticacaoApplication, AutenticacaoApplication>();
builder.Services.AddScoped<IDashboardProjetosApplication, DashboardProjetosApplication>();

// Adicione as interfaces de banco de dados
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
builder.Services.AddScoped<IHistoriaRepository, HistoriaRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IUsuarioEquipeRepository, UsuarioEquipeRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<ITwoFactorRepository, TwoFactorRepository>();
builder.Services.AddScoped<IDashboardProjetosRepository, DashboardProjetosRepository>();

// Adiciona os serviços
// builder.Services.AddScoped<IJsonPlaceHolderService, JsonPlaceHolderService>();
builder.Services.AddScoped<IEmailService, EmailService>();  

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .SetIsOriginAllowedToAllowWildcardSubdomains()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var dbPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "..",
    "Projeto360.Repository",
    "projeto360.sqlite"
);

builder.Services.AddDbContext<Projeto360Context>(options =>
    options.UseSqlite($"Data Source={dbPath}")
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();