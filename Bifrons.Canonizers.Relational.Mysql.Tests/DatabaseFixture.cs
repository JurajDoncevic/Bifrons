using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bifrons.Canonizers.Relational.Mysql.Tests;

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
        var env_mysqlUser = _configuration.GetValue<string>("DatabaseContainer:Env:MYSQL_USER");
        var env_mysqlPassword = _configuration.GetValue<string>("DatabaseContainer:Env:MYSQL_PASSWORD");
        var env_mysqlDatabase = _configuration.GetValue<string>("DatabaseContainer:Env:MYSQL_DB");

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
            .WithEnvironment("MYSQL_USER", env_mysqlUser)
            .WithEnvironment("MYSQL_PASSWORD", env_mysqlPassword)
            .WithEnvironment("MYSQL_DB", env_mysqlDatabase)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(containerPort))
            .Build();

        _services = new ServiceCollection();
        var connectionString = $"Server=localhost;Port={hostPort};Database={env_mysqlDatabase};Uid={env_mysqlUser};Pwd={env_mysqlPassword};";
        // add services here
        _services.AddScoped<QueryManager>(_ => QueryManager.Cons(connectionString).Data ?? throw new Exception("QueryManager failed to initialize"));
        _services.AddScoped<CommandManager>(_ => CommandManager.Cons(connectionString).Data ?? throw new Exception("CommandManager failed to initialize"));
        _services.AddScoped<MetadataManager>(_ => MetadataManager.Cons(connectionString).Data ?? throw new Exception("MetadataManager failed to initialize"));

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
