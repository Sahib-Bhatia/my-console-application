using System.Text.Json;
using ConsoleApp;
using RestSharp;

var client = new RestClient(); 

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

static List<Repository> ProcessRepositoriesAsync (RestClient client)
{
    var request = new RestRequest("https://api.github.com/orgs/dotnet/repos");
    var response = client.ExecuteGet(request);
    var data = JsonSerializer.Deserialize<List<Repository>>(response.Content!);

    return data ?? new List<Repository>();
}

