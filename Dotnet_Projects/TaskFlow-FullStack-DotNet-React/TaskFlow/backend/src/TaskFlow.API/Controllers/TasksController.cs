using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // every endpoint here requires a valid JWT; role checks are per-action below
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TasksController(ITaskService taskService) => _taskService = taskService;

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null,
        [FromQuery] WorkStatus? status = null, [FromQuery] Priority? priority = null, CancellationToken ct = default)
    {
        var query = new TaskQueryParams(page, pageSize, status, priority, search);
        var result = await _taskService.GetTasksAsync(query, ct);
        return Ok(ApiResponse<PagedResult<TaskItemDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var task = await _taskService.GetByIdAsync(id, ct);
        if (task is null) return NotFound(ApiResponse<object>.Fail("Task not found."));
        return Ok(ApiResponse<TaskItemDto>.Ok(task));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request, CancellationToken ct)
    {
        var task = await _taskService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, ApiResponse<TaskItemDto>.Ok(task, "Task created."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request, CancellationToken ct)
    {
        var task = await _taskService.UpdateAsync(id, request, ct);
        if (task is null) return NotFound(ApiResponse<object>.Fail("Task not found or access denied."));
        return Ok(ApiResponse<TaskItemDto>.Ok(task, "Task updated."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _taskService.DeleteAsync(id, ct);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Task not found or access denied."));
        return Ok(ApiResponse<object>.Ok(new { }, "Task deleted."));
    }
}
