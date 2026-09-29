using Inventory.Api.DTOs;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(
        IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees =
            await _employeeService.GetAllAsync();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee =
            await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        CreateEmployeeDto dto)
    {
        var employee =
            await _employeeService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employee
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateEmployeeDto dto)
    {
        var employee =
            await _employeeService.UpdateAsync(id, dto);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _employeeService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
