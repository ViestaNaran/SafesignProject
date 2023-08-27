using Microsoft.AspNetCore;
using Safesign.Data;
using Safesign.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Safesign.Services;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetSection("Cosmos").Get<CosmosConnection>();
builder.Services.AddSingleton(connection);
builder.Services.AddSingleton<PizzaService>();
builder.Services.AddSingleton<PlanService>();
builder.Services.AddSingleton<SignService>();
builder.Services.AddSingleton<ConstructionSiteService>();

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
