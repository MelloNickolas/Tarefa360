using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projeto360.Application;
using Projeto360.Repositories;
using Projeto360.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<ITarefaAplicacao, TarefaAplicacao>();

// Interfaces de banco
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IJsonPlaceHolderService, JsonPlaceHolderService>();

// Banco de dados
builder.Services.AddDbContext<Projeto360Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS precisa ficar aqui
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ usar o CORS
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();