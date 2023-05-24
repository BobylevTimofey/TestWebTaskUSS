using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DeckContext>(opt =>
    opt.UseInMemoryDatabase("DeckList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISorter, ManualSort>();
builder.Services.AddTransient<ISorter, FisherYatesSort>();
builder.Services.AddTransient<SortService>();
builder.Services.AddTransient<DeckService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
