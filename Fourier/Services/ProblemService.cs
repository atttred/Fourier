namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public interface IProblemService
{
    Task<IEnumerable<Problem>> GetAllTasksAsync();
    Task<Problem> GetTaskByIdAsync(Guid id);
    Task<Problem> CreateTaskAsync(Problem task);
    Task UpdateTaskAsync(Problem task);
    Task DeleteTaskAsync(Guid id);
    Task UpdateProgress(Guid id, int progress);
}

public class ProblemService : IProblemService
{
    private readonly IProblemRepository _taskRepository;
    private readonly IHubContext<ProgressHub> _hubContext;

    public ProblemService(IProblemRepository taskRepository, IHubContext<ProgressHub> hubContext)
    {
        _taskRepository = taskRepository;
        _hubContext = hubContext;
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

    public async Task UpdateProgress(Guid id, int progress)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        task.Progress = progress;
        await _taskRepository.UpdateAsync(task);
        await _hubContext.Clients.Group(id.ToString()).SendAsync("UpdateProgress", progress);
    }
}