var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Seu Projeto API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => 
    {
        builder.WithOrigins("http://localhost:3000") // Adicione a origem do seu aplicativo front-end
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient(); // Adicione esta linha para registrar o HttpClient

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seu Projeto API v1"));
}

app.UseCors(); // Adicione esta linha para habilitar o CORS

app.UseHttpsRedirection();

app.MapControllers(); // Adicione esta linha para mapear os controladores

app.Run();
