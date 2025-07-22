using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiBancoDeDadosETestes.Dominio.DTOs;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;
using MinimalApiBancoDeDadosETestes.Dominio.Enuns;
using MinimalApiBancoDeDadosETestes.Dominio.Interfaces;
using MinimalApiBancoDeDadosETestes.Dominio.ModelViews;
using MinimalApiBancoDeDadosETestes.Dominio.Servicos;
using MinimalApiBancoDeDadosETestes.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdmnistradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

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

app.MapGet("/mensagem", () => Results.Json(new Home())).WithTags("Home");

app.MapPost("administradores/login",
    ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
    {
        if (administradorServico.Login(loginDTO) != null)
            return Results.Ok("Login efetuado com sucesso");
        else
            return Results.Unauthorized();

    }).WithTags("Administradores");

app.MapGet("/administradores",
    ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
    {
        var adms = new List<AdministradorModelView>();
        var administradores = administradorServico.Todos(pagina);

        foreach (var adm in administradores)
        {
            adms.Add(new AdministradorModelView {
                Id = adm.Id,
                Email = adm.Email,
                Perfil = adm.Perfil
            
            });
        }

        return Results.Ok(adms);

    }).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscaPorId(id);

    if (administrador == null) return Results.NotFound();

    return Results.Ok(new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });

}).WithTags("Administradores");

app.MapPost("/administradores",
    ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
    {
        var validacao = new ErrosDeValidacao
        {
            Mensagens = new List<string> { }
        };

        if (string.IsNullOrEmpty(administradorDTO.Email))
            validacao.Mensagens.Add("O campo Email não pode ser vazio");

        if (string.IsNullOrEmpty(administradorDTO.Senha))
            validacao.Mensagens.Add("O campo Senha não pode ser vazio");

        if (administradorDTO.Perfil == null)
            validacao.Mensagens.Add("O campo perfil não pode ser vazio");

        if (validacao.Mensagens.Count > 0)
            return Results.BadRequest(validacao);

        var administrador = new Administrador
        {
            Email = administradorDTO.Email,
            Senha = administradorDTO.Senha,
            Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
        };

        administradorServico.Incluir(administrador);

        return Results.Created($"/administrador/{administrador.Id}", new AdministradorModelView
        {
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });

    }).WithTags("Administradores");
    
ErrosDeValidacao validacaoDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string> { }
    };
        

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome não pode ser vazio");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("É necessário preencher a marca do veículo");

    if (veiculoDTO.Ano < 1950)
        validacao.Mensagens.Add("Veículo muito antigo. Aceito somente anos superiores a 1950");

    return validacao;
}

app.MapPost("/veiculos",
    ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
    {
        var validacao = validacaoDTO(veiculoDTO);

        if (validacao.Mensagens.Count > 0)
            return Results.BadRequest(validacao);

        var veiculo = (new Veiculo
        {
            Nome = veiculoDTO.Nome,
            Marca = veiculoDTO.Marca,
            Ano = veiculoDTO.Ano
        });

        veiculoServico.Incluir(veiculo);

        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
        
    }).WithTags("Veículos");

app.MapGet("/veiculos", 
    ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
    {
        var veiculos = veiculoServico.Todos(pagina);

        return Results.Ok(veiculos);

    }).WithTags("Veículos");

app.MapGet("/veiculos/{id}",
    ([FromRoute] int id, IVeiculoServico veiculoServico) =>
    {
    var veiculo = veiculoServico.BuscaPorId(id);

        if (veiculo == null) return Results.NotFound();

        return Results.Ok(veiculo);

    }).WithTags("Veículos");

app.MapPut("/veiculos/{id}",
    ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
    {
        var veiculo = veiculoServico.BuscaPorId(id);

        if (veiculo == null) return Results.NotFound();

        var validacao = validacaoDTO(veiculoDTO);

        if (validacao.Mensagens.Count > 0)
            return Results.BadRequest(validacao);        

        veiculo.Nome = veiculoDTO.Nome;
        veiculo.Marca = veiculoDTO.Marca;
        veiculo.Ano = veiculoDTO.Ano;

        veiculoServico.Atualizar(veiculo);

        return Results.Ok(veiculo);

    }).WithTags("Veículos");

app.MapDelete("/veiculos/{id}",
    ([FromRoute] int id, IVeiculoServico veiculoServico) =>
    {
        var veiculo = veiculoServico.BuscaPorId(id);

        if (veiculo == null) return Results.NotFound();

        veiculoServico.Apagar(veiculo);

        return Results.NoContent();

    }).WithTags("Veículos");

app.Run();