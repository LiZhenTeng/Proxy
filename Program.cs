var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var allowOrigions = builder.Configuration.GetSection("AllowOrigins").Value?.Split(',');
if (allowOrigions != null && allowOrigions.Length > 0)
{
    app.UseCors(o => o.WithOrigins(allowOrigions).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
}


app.UseAuthorization();

app.MapControllers();

app.Run();
