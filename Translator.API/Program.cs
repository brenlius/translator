using Scalar.AspNetCore;
using Translator.Design.Application.IRepositories;
using Translator.Design.Application.IServices;
using Translator.Design.Application.Services;
using Translator.Design.Infrastracture.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS to allow Blazor to access the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["BlazorBaseUrl"]?.ToString() ?? string.Empty)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.AddScoped<ITranslateService, TranslateService>();
builder.Services.AddScoped<ITranslateRepository, TranslateRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
