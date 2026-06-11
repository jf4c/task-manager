using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Mappers;
using TaskManager.Api.Requests;
using TaskManager.Api.Responses;
using TaskManager.Application.Abstractions;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de tarefas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TaskItemController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    /// <summary>
    /// Cria uma nova tarefa.
    /// </summary>
    /// <param name="request">Dados da tarefa a ser criada.</param>
    /// <response code="200">Tarefa criada com sucesso.</response>
    /// <response code="400">Dados da requisição inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var dto = request.ToCreateTaskDTO();
        await _taskService.CreateTaskAsync(dto);
        return Ok();
    }

    /// <summary>
    /// Retorna todas as tarefas.
    /// </summary>
    /// <response code="200">Lista de tarefas retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<TaskItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TaskItemResponse>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTaskItemsAsync();
        var response = tasks.Select(x => x.ToTaskItemResponse()).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Retorna uma tarefa pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <response code="200">Tarefa encontrada e retornada com sucesso.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemResponse>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskItemByIdAsync(id);
        return Ok(task.ToTaskItemResponse());
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <param name="request">Dados atualizados da tarefa.</param>
    /// <response code="204">Tarefa atualizada com sucesso.</response>
    /// <response code="400">Dados da requisição inválidos.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
    {
        var dto = request.ToUpdateTaskDTO(id);
        await _taskService.UpdateTaskAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Remove uma tarefa pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <response code="204">Tarefa removida com sucesso.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }
}
