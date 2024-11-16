namespace Fourier_Balancer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

public class CustomLoadPolicy : ILoadBalancingPolicy
{
    private readonly HttpClient _httpClient;
    private readonly ConcurrentDictionary<string, int> _serverTaskCounts = new();
    private int _roundRobinCounter = 0;

    public CustomLoadPolicy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string Name => "CustomLoadPolicy";

    public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
    {
        if (availableDestinations.Count == 0)
        {
            return null;
        }

        if (context.Request.Path.StartsWithSegments("/api/Problem/create"))
        {
            return GetLessBusyServer(availableDestinations);
        }
        else
        {
            return GetRoundRobinServer(availableDestinations);
        }
    }

    private DestinationState GetLessBusyServer(IReadOnlyList<DestinationState> availableDestinations)
    {
        DestinationState lessBusyServer = availableDestinations[0];
        int minInput = int.MaxValue;

        foreach (var destination in availableDestinations)
        {
            var inputWeight = GetInputWeight(destination.Model.Config.Address).Result;
            _serverTaskCounts.AddOrUpdate(destination.Model.Config.Address, inputWeight, (_, _) => inputWeight);

            if (inputWeight < minInput)
            {
                minInput = inputWeight;
                lessBusyServer = destination;
            }

            Console.WriteLine($"Server: {destination.Model.Config.Address} has {inputWeight} input value");
        }

        return lessBusyServer;
    }

    private DestinationState GetRoundRobinServer(IReadOnlyList<DestinationState> availableDestinations)
    {
        var destination = availableDestinations[_roundRobinCounter];
        _roundRobinCounter = (_roundRobinCounter + 1) % availableDestinations.Count;
        return destination;
    }

    private async Task<int> GetInputWeight(string address)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{address}/api/Problem/problemInProgress");
            Console.WriteLine($"\n\nServer: {address} has {response.StatusCode} status code");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Server: {address} has {content} input value(GetInputWeight)");

                return JsonSerializer.Deserialize<int>(content);
            }
            else
            {
                return 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\n\n\n\n\nError while getting input weight");
            Console.WriteLine($"Error: {e.Message}");
        }
        return 0;
    }
}