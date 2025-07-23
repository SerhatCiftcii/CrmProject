var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger servislerini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP request pipeline konfigürasyonu
if (app.Environment.IsDevelopment())
{
    // Swagger middleware'lerini devreye al
    app.UseSwagger(); // swagger.json oluşturur
    app.UseSwaggerUI(); // Swagger UI arayüzünü açar
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
