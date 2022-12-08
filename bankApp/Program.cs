using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  
builder.Services.AddSession(); 
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddHttpContextAccessor();  
builder.Services.AddControllersWithViews();
builder.Services.AddSession(opts =>
{
    
    opts.IdleTimeout = TimeSpan.FromHours(5);
});
builder.Services.AddCors(o => o.AddPolicy("CORS", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 
app.UseCors("CORS");
app.UseHttpsRedirection(); 
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
