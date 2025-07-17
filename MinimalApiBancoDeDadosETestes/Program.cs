var builder = WebApplication.CreateBuilder(args);

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
    (MinimalApiBancoDeDadosETestes.Dominio.DTOs.LoginDTO loginDTO) =>
    {
        if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
            return Results.Ok("Login efetuado com sucesso");
        else
            return Results.Unauthorized();
    });

app.Run();