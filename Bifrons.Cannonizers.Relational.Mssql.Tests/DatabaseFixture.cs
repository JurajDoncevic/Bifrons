using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bifrons.Cannonizers.Relational.Mssql.Tests;

public sealed class DatabaseFixture : IDisposable
{
    private readonly IContainer? _databaseContainer;

    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;
    private readonly IServiceScope _scope;

    public DatabaseFixture()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var buildImage = _configuration.GetValue<bool>("DatabaseContainer:BuildImage");
        var hostPort = _configuration.GetValue<int>("DatabaseContainer:HostPort");
        var containerPort = _configuration.GetValue<int>("DatabaseContainer:ContainerPort");
        var imageName = _configuration.GetValue<string>("DatabaseContainer:ImageName");
        var containerName = _configuration.GetValue<string>("DatabaseContainer:ContainerName");
        var env_mssqlUser = _configuration.GetValue<string>("DatabaseContainer:Env:MSSQL_USER");
        var env_mssqlPassword = _configuration.GetValue<string>("DatabaseContainer:Env:MSSQL_PASSWORD");
        var env_mssqlDatabase = _configuration.GetValue<string>("DatabaseContainer:Env:MSSQL_DB");

        // Optionally build the image from the Dockerfile
        if (buildImage)
        {
            new ImageFromDockerfileBuilder()
                .WithDeleteIfExists(true)
                .WithCleanUp(true)
                .WithDockerfileDirectory(CommonDirectoryPath.GetProjectDirectory(), string.Empty)
                .WithDockerfile("Dockerfile")
                .WithName(imageName)
                .Build().CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // Create the container
        _databaseContainer = new ContainerBuilder()
            .WithImage(imageName)
            .WithName(containerName)
            .WithPortBinding(hostPort, containerPort)
            .WithEnvironment("MSSQL_USER", env_mssqlUser)
            .WithEnvironment("MSSQL_PASSWORD", env_mssqlPassword)
            .WithEnvironment("MSSQL_DB", env_mssqlDatabase)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(containerPort))
            .Build();

        _services = new ServiceCollection();
        var connectionString = $"Server=localhost,{hostPort};Database={env_mssqlDatabase};User Id={env_mssqlUser};Password={env_mssqlPassword};";
        // add services here
        _services.AddScoped<QueryManager>(_ => new QueryManager(connectionString));
        _services.AddScoped<CommandManager>(_ => new CommandManager(connectionString));
        _services.AddScoped<MetadataManager>(_ => new MetadataManager(connectionString));

        _scope = _services.BuildServiceProvider().CreateScope();

        _databaseContainer?.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    }

    public T GetService<T>()
        where T : class
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    public async void Dispose()
    {
        _scope.Dispose();

        if (_databaseContainer != null)
            await _databaseContainer.DisposeAsync();
    }
}
