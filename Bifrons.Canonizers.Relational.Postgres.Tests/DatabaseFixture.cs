﻿using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bifrons.Canonizers.Relational.Postgres.Tests;

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
        var env_postgresUser = _configuration.GetValue<string>("DatabaseContainer:Env:POSTGRES_USER");
        var env_postgresPassword = _configuration.GetValue<string>("DatabaseContainer:Env:POSTGRES_PASSWORD");
        var env_postgresDatabase = _configuration.GetValue<string>("DatabaseContainer:Env:POSTGRES_DB");

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
            .WithEnvironment("POSTGRES_USER", env_postgresUser)
            .WithEnvironment("POSTGRES_PASSWORD", env_postgresPassword)
            .WithEnvironment("POSTGRES_DB", env_postgresDatabase)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(containerPort))
            .Build();

        _services = new ServiceCollection();
        var connectionString = $"Host=localhost;Port={hostPort};Username={env_postgresUser};Password={env_postgresPassword};Database={env_postgresDatabase}";
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
