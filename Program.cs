using BoykisserBot.Configuration;
using Microsoft.Extensions.Configuration;

namespace BoykisserBot;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }

    private async Task MainAsync()
    {
        DotEnv.Load(new FileInfo(".env"));
        Config config = new(
            new ConfigurationBuilder()
                .AddJsonFile("config/appsettings.json")
                .AddEnvironmentVariables()
                .Build()
        );
    }
}