using Safesign.Data;
using Safesign.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetSection("Cosmos").Get<CosmosConnection>();
builder.Services.AddSingleton(connection);
builder.Services.AddSingleton<PizzaService>();
builder.Services.AddSingleton<PlanService>();
builder.Services.AddSingleton<SignService>();
builder.Services.AddSingleton<ConstructionSiteService>();
builder.Services.AddSingleton<SensorService>();

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
