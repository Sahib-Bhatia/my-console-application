using System.Text.Json;
using ConsoleApp;
using RestSharp;
using Serilog;

var client = new RestClient();

Log.Logger = new LoggerConfiguration().WriteTo.Console().MinimumLevel.Debug().CreateLogger();

var repositories = ProcessRepositoriesAsync(client);

foreach (var repository in repositories)
{
    Console.WriteLine($"Name: {repository.Name}");
    Console.WriteLine($"Homepage: {repository.Homepage}");
    Console.WriteLine($"GitHub: {repository.GitHubHomeUrl}");
    Console.WriteLine($"Description: {repository.Description}");
    Console.WriteLine($"Watchers: {repository.Watchers:#,0}");
    Console.WriteLine($"{repository.LastPush}");
    Console.WriteLine();
}

static List<Repository> ProcessRepositoriesAsync(RestClient client)
{
    Log.Information("Sending a get request to the github api");
    var request = new RestRequest("https://api.github.com/orgs/dotnet/repos");
    var response = client.ExecuteGet(request);
    if (!response.IsSuccessful)
    {
        Log.Error($"Something went wrong. Error Message {response.StatusCode}");
        return null;
    }
    Log.Information("Data Fetching Successful");
    var data = JsonSerializer.Deserialize<List<Repository>>(response.Content!);

    return data ?? new List<Repository>();
}

