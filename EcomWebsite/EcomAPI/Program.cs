using EcomAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var apiCorsPolicy = "ApiCorsPolicy";
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:7119", "https://localhost:7119")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                          //.WithMethods("OPTIONS", "GET");
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   //pp.UseMvcWithDefaultRoute();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
