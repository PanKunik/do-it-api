﻿using DoIt.Api.Controllers.Tasks;
using DoIt.Api.Domain.Tasks;
using DoIt.Api.Persistence.Repositories;
using DoIt.Api.Shared;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Services.Tasks;

public class TasksService(ITasksRepository repository)
    : ITasksService
{
    private readonly ITasksRepository _repository = repository;

    public async System.Threading.Tasks.Task<List<TaskDTO>> GetAll()
    {
        var result = await _repository.GetAll();
        return result
            .Select(r => r.ToDto())
            .ToList();
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> GetById(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var result = await _repository.GetById(taskIdResult.Value!);

        return result.Map<Result<TaskDTO>>(
            onSuccess: value => value.ToDto(),
            onFailure: error => error
        );
    }

    public async System.Threading.Tasks.Task<Result<TaskDTO>> Create(CreateTaskRequest request)
    {
        var taskTitleResult = Title.CreateFrom(request.Title);

        if (taskTitleResult.IsFailure)
            return taskTitleResult.Error!;

        var taskResult = Task.Create(
            TaskId.CreateUnique(),
            taskTitleResult.Value!,
            DateTime.UtcNow,
            false,
            false
        );

        if (taskResult.IsFailure)
            return taskResult.Error!;

        var createTaskResult = await _repository.Create(taskResult.Value!);

        if (createTaskResult.IsFailure)
            return createTaskResult.Error!;

        return createTaskResult.Value!.ToDto();
    }

    public async System.Threading.Tasks.Task<Result> Delete(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        return await _repository.Delete(taskIdResult.Value!);
    }

    public async System.Threading.Tasks.Task<Result> Update(
        Guid id,
        UpdateTaskRequest request
    )
    {
        var taskIdResult = TaskId.CreateFrom(id);

        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var titleResult = Title.CreateFrom(request.Title);

        if (titleResult.IsFailure)
            return titleResult.Error!;

        var taskToUpdateResult = await _repository.GetById(taskIdResult.Value!);

        if (taskToUpdateResult.IsFailure)
            return taskToUpdateResult.Error!;

        var taskToUpdate = taskToUpdateResult.Value!;
        taskToUpdate.UpdateTitle(titleResult.Value!);

        return await _repository.Update(taskToUpdate);
    }

    public async Task<Result> ChangeState(Guid id)
    {
        var taskIdResult = TaskId.CreateFrom(id);
        
        if (taskIdResult.IsFailure)
            return taskIdResult.Error!;

        var taskToDoResult = await _repository.GetById(taskIdResult.Value!);

        if (taskToDoResult.IsFailure)
            return taskToDoResult.Error!;

        var taskToDo = taskToDoResult.Value!;
        taskToDo.ChangeState();

        await _repository.Update(taskToDo);
        
        return Result.Success();
    }
}
