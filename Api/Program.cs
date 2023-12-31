using Api.Configurations;
using Api.Data;
using Api.Dtos.MappingProfiles;
using Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IDependentsService, DependentsService>();
builder.Services.AddScoped<IEmployeeDataRepository, EmployeeDataRepository>();
builder.Services.AddTransient<IBenefitCalculator, BaseBenefitCalculator>();
builder.Services.AddTransient<IBenefitCalculator, HighSalaryBenefitCalculator>();
builder.Services.AddTransient<IBenefitCalculator, DependentAgeSeniorThresholdBenefitCalculator>();
builder.Services.AddTransient<IPayrollService, PayrollService>();

builder.Services.Configure<PayrollConfig>(builder.Configuration.GetSection("PayrollConfig"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowLocalhost,
        policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
