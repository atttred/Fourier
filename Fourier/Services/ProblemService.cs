namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProblemService
{
    Task<IEnumerable<Problem>> GetAllTasksAsync();
    Task<Problem> GetTaskByIdAsync(Guid id);
    Task<Problem> CreateTaskAsync(Problem task);
    Task UpdateTaskAsync(Problem task);
    Task DeleteTaskAsync(Guid id);
}

public class ProblemService : IProblemService
{
    private readonly ITaskRepository _taskRepository;

    public ProblemService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<Problem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<Problem> GetTaskByIdAsync(Guid id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<Problem> CreateTaskAsync(Problem task)
    {
        task.Id = Guid.NewGuid();
        task.Status = "Pending";
        task.Progress = 0;
        return await _taskRepository.AddAsync(task);
    }

    public async Task UpdateTaskAsync(Problem task)
    {
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        await _taskRepository.DeleteAsync(id);
    }
}