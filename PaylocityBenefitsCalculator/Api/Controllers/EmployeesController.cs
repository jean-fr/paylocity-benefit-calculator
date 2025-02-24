using Api.Dtos;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPaycheckService _paycheckService;
    private readonly IMapper _mapper;
     private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeService employeeService, IPaycheckService paycheckService,IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeService = employeeService;
        _paycheckService = paycheckService;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        var employeeDto = _mapper.Map<GetEmployeeDto>(employee);
        employeeDto.Dependents.AddRange(employee.Dependents.Select(d => _mapper.Map<GetDependentDto>(d)).ToList());

        return new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto
        };
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        var employeesDto = new List<GetEmployeeDto>();

        foreach (var employee in employees)
        {
            var employeeDto = _mapper.Map<GetEmployeeDto>(employee);
            employeeDto.Dependents.AddRange(employee.Dependents.Select(d => _mapper.Map<GetEmployeeListDependentDto>(d)).ToList());
            employeesDto.Add(employeeDto);
        }

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeesDto
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get employee paycheck by employee Id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> GetPaycheck(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if(employee==null) return NotFound();

        var paycheck = await _paycheckService.GeneratePaycheckAsync(employee);

        return new ApiResponse<GetEmployeePaycheckDto>
        {
            Data = _mapper.Map<GetEmployeePaycheckDto>(paycheck)
        };
    }
}
