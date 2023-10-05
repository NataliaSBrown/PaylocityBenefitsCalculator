using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;
    private readonly IPayrollService _payrollService;

    public EmployeesController(IEmployeesService employeesService, IPayrollService payrollService)
    {
        _employeesService = employeesService;
        _payrollService = payrollService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            var employee = await _employeesService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new ApiResponse<GetEmployeeDto>
                {
                    Success = false,
                    Error = $"Employee is not found with specified ID {id}"
                });
            }

            return Ok(new ApiResponse<GetEmployeeDto>
            {
                Success = true,
                Data = employee
            });

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An error occurred while fetching data for {nameof(Get)}: {ex.Message}");
        }
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            var employees = await _employeesService.GetEmployeesAsync();

            if (employees == null || !employees.Any())
            {
                return NotFound(new ApiResponse<List<GetEmployeeDto>>
                {
                    Success = false,
                    Error = "No employees found"
                });
            }

            return Ok(new ApiResponse<List<GetEmployeeDto>>
            {
                Success = true,
                Data = employees
            });

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An error occurred while fetching data for {nameof(GetAll)}: {ex.Message}");
        }
    }

    [SwaggerOperation(Summary = "Calculate Employee's paycheck")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> GetEmployeePaycheck(int id)
    {
        try
        {
            var employee = await _employeesService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new ApiResponse<GetEmployeeDto>
                {
                    Success = false,
                    Error = $"Employee is not found with specified ID {id}"
                });
            }

            GetEmployeePaycheckDto paycheck = _payrollService.CalculatePaycheck(employee);

            return Ok(new ApiResponse<GetEmployeePaycheckDto>
            {
                Success = true,
                Data = paycheck
            });

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An error occurred while fetching data for {nameof(GetEmployeePaycheck)}: {ex.Message}");
        }
    }
}
