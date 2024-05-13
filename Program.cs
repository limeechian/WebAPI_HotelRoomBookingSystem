using HotelRoomManagementApp;
using HotelRoomManagementApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using HotelRoomManagementApp.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiKeyAuthorizationFilter>();
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelRoomManagementApp API", Version = "v1" });

    // Add your custom filter here if needed - add required header parameter 
    c.OperationFilter<ApiKeyOperationFilter>();


});

builder.Services.AddCors(options =>
     {
         options.AddDefaultPolicy(builder =>
         {
             builder.WithOrigins("http://localhost:4200") // Update with your Angular app's URL
                 .AllowAnyHeader()
                 .AllowAnyMethod();
         });
     });

builder.Services.AddDbContext<LEC2023DbContext>(
    o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString"),  
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable developer exception page - middleware used during development to provide detailed error information when an unhandled exception occurs in the application
    app.UseDeveloperExceptionPage();

    // Enable Swagger
    app.UseSwagger();

    app.UseCors();

    // Configure Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelRoomManagementApp API v1");

    });
} 
else
{
    // In a production environment, configure error handling differently,
    // such as using a custom error page or logging errors.
    app.UseExceptionHandler("");  // e.g., "/Home/Error" - the custom error page
    app.UseHsts();  // Apply HTTP Strict Transport Security (HSTS) in production. 
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Enable authentication

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

