namespace Fourier.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public interface ILogicService
{
    Task<string> CalculateAsync(Guid problemId, int input);
}

public class LogicService : ILogicService
{
    private readonly IServiceProvider _serviceProvider;

    public LogicService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> CalculateAsync(Guid problemId, int input)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var problemService = scope.ServiceProvider.GetRequiredService<IProblemService>();
            var cancellationTokenService = scope.ServiceProvider.GetRequiredService<ICancellationTokenService>();

            var startTime = DateTime.Now;
            await problemService.UpdateStatusAsync(problemId, "In progress", startTime, null);

            int N = input;
            double a0 = 0;
            double[] a = new double[N];
            double[] b = new double[N];
            double interval = 2 * Math.PI;
            int numSamples = 1000;
            double dx = interval / numSamples;

            for (int n = 1; n <= N; n++)
            {
                var token = await cancellationTokenService.GetCancellationTokenAsync(problemId);
                Console.WriteLine($"Token is cancelled: {token?.IsCancelled}");
                if (token?.IsCancelled == true)
                {
                    await problemService.UpdateStatusAsync(problemId, "Cancelled", startTime, DateTime.Now);
                    return "Cancelled";
                }

                double sumA = 0;
                double sumB = 0;

                for (int i = 0; i < numSamples; i++)
                {
                    double x = -Math.PI + i * dx;
                    double fx = Math.Sin(x);

                    sumA += fx * Math.Cos(n * x) * dx;
                    sumB += fx * Math.Sin(n * x) * dx;
                }

                a[n - 1] = sumA / Math.PI;
                b[n - 1] = sumB / Math.PI;

                await Task.Delay(1000);

                int progress = (int)((double)n / N * 100);
                await problemService.UpdateProgress(problemId, progress);
            }

            var result = $"Fourier series with {N} terms: \n";
            result += $"a0/2 = {a0 / 2} \n";
            for(int n = 1; n <= N; n++)
            {
                result += $"a{n} * cos({n}x) + b{n} * sin({n}x)\n";
            }

            await problemService.UpdateStatusAsync(problemId, "Finished", startTime, DateTime.Now);
            await problemService.UpdateResultAsync(problemId, result);

            return result;
        }
    }
}