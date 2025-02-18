using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;
    private readonly IMapper _mapper;

    public DependentsController(IDependentService dependentService, IMapper mapper)
    {
        _dependentService = dependentService;
        _mapper = mapper;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetByIdAsync(id);

        if (dependent == null)
        {
            return NotFound();
        }

        var dependentDto = _mapper.Map<GetDependentDto>(dependent);

        return new ApiResponse<GetDependentDto>
        {
            Data = dependentDto
        };
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllAsync();

        return new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents.Select(d => _mapper.Map<GetDependentDto>(d)).ToList()
        };
    }
}
