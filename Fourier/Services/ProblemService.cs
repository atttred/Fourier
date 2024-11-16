namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fourier.DTOs;

public interface IProblemService
{
    Task<IEnumerable<Problem>> GetAllTasksAsync();
    Task<Problem> GetTaskByIdAsync(Guid id);
    Task<Problem> CreateTaskAsync(ProblemDto task, Guid userId);
    Task UpdateTaskAsync(Problem task);
    Task DeleteTaskAsync(Guid id);
    Task UpdateProgress(Guid id, int progress);
    Task UpdateStatusAsync(Guid id, string status, DateTime startedAt, DateTime? endTime);
    Task UpdateResultAsync(Guid id, string result);
    Task<IEnumerable<Problem>> GetAllUserTasksAsync(Guid userId);
}

public class ProblemService : IProblemService
{
    private readonly IProblemRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public ProblemService(IProblemRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Problem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<Problem> GetTaskByIdAsync(Guid id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<Problem> CreateTaskAsync(ProblemDto task, Guid userId)
    {
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = "Pending",
            Input = task.Input,
            StartedAt = null,
            FinishedAt = null,
            Progress = 0,
            Result = string.Empty,
            IsCancelled = false,
            User = _userRepository.GetByIdAsync(userId).Result
        };

        return await _taskRepository.AddAsync(problem);
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
    }

    public async Task UpdateStatusAsync(Guid id, string status, DateTime startedAt, DateTime? endTime)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        task.Status = status;
        task.StartedAt = startedAt;
        task.FinishedAt = endTime;
        await _taskRepository.UpdateAsync(task);
    }

    public async Task UpdateResultAsync(Guid id, string result)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        task.Result = result;
        await _taskRepository.UpdateAsync(task);
    }

    public async Task<IEnumerable<Problem>> GetAllUserTasksAsync(Guid userId)
    {
        return await _taskRepository.GetAll(userId);
    }
}