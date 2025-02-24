using Api.Dtos;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Repositories;
using Api.Repositories.Entities;
using Api.Services;
using Api.Services.Models;
using AutoMapper;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IDependentRepository, DependentRepository>();
builder.Services.AddTransient<IDependentService, DependentService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IPaycheckService, PaycheckService>();

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    // dao to service
    cfg.CreateMap<EntityBase, ServiceResponseBase>()
    .IncludeAllDerived();
    cfg.CreateMap<Employee, EmployeeResponse>().ForMember(e => e.Dependents, act => act.Ignore());
    cfg.CreateMap<Dependent, DependentResponse>().ForMember(d => d.EmployeeName, act => act.Ignore()).ForMember(d => d.EmployeeId, act => act.Ignore()).ForMember(d => d.Relationship, act => act.Ignore());


    // service to api response
    cfg.CreateMap<ServiceResponseBase, DtoBase>()
    .IncludeAllDerived();

    cfg.CreateMap<EmployeeResponse, GetEmployeeDto>().ForMember(e => e.Dependents, act => act.Ignore());
    cfg.CreateMap<DependentResponse, GetDependentDto>();
    cfg.CreateMap<DependentResponse, GetEmployeeListDependentDto>();
    cfg.CreateMap<PaycheckResponse, GetEmployeePaycheckDto>();

});
var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

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
