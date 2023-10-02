using FavouriteManager.Data;
using FavouriteManager.Services;
using FavouriteManager.Middleware;
using Microsoft.EntityFrameworkCore;
using FavouriteManager.Services.implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFavouriteService, FavouriteService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var connectionString = builder.Configuration.GetConnectionString("AppDBConnectionString");

builder.Services.AddDbContext<AppDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseExceptionHandler("/error");
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.Run();
