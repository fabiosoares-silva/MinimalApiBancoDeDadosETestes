using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiBancoDeDadosETestes.Dominio.DTOs;
using MinimalApiBancoDeDadosETestes.Dominio.Interfaces;
using MinimalApiBancoDeDadosETestes.Dominio.Servicos;
using MinimalApiBancoDeDadosETestes.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdmnistradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  

app.MapGet("/mensagem", () => "Hello World!");

app.MapPost("/login",
    ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
    {
        if (administradorServico.Login(loginDTO) != null)
            return Results.Ok("Login efetuado com sucesso");
        else
            return Results.Unauthorized();
    });

app.Run();