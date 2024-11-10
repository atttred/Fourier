using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ProgressHub : Hub
{
    public async Task JoinProblemGroup(string problemId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, problemId);
    }

    public async Task LeaveProblemGroup(string problemId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, problemId);
    }
}
