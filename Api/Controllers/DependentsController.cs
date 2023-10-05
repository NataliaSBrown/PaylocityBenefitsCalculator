using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentsService _dependentsService;

    public DependentsController(IDependentsService dependentsService)
    {
        _dependentsService = dependentsService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            var dependent = await _dependentsService.GetDependentByIdAsync(id);

            if (dependent == null)
            {
                return NotFound(new ApiResponse<GetDependentDto>
                {
                    Success = false,
                    Error = $"Dependent is not found with specified ID {id}"
                });
            }

            return Ok(new ApiResponse<GetDependentDto>
            {
                Success = true,
                Data = dependent
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<List<GetEmployeeDto>>
            {
                Success = false,
                Error = $"An error occurred while fetching data for {nameof(Get)}",
                Message = ex.Message
            });
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        try
        {
            var dependents = await _dependentsService.GetDependentsAsync();

            if (dependents == null || !dependents.Any())
            {
                return NotFound(new ApiResponse<List<GetDependentDto>>
                {
                    Success = false,
                    Error = "No dependents found"
                });
            }

            return Ok(new ApiResponse<List<GetDependentDto>>
            {
                Success = true,
                Data = dependents
            });

        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<List<GetEmployeeDto>>
            {
                Success = false,
                Error = $"An error occurred while fetching data for {nameof(GetAll)}",
                Message = ex.Message
            });
        }
    }
}
