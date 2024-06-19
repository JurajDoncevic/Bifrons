using System.Text.Json.Serialization;
using Bifrons.Cannonizers.Relational;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bifrons.Experiments;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum DatabaseTypes
{
    MYSQL,
    POSTGRES
}


public sealed class DatabaseFixture : IDisposable
{

    private readonly IContainer? _academicDatabaseContainer;
    private readonly IContainer? _financialDatabaseContainer;

    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;
    private readonly IServiceScope _scope;

    public DatabaseFixture()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var academic_buildImage = _configuration.GetValue<bool>("DatabaseContainers:AcademicManagement:BuildImage");
        var academic_databaseType = _configuration.GetValue<DatabaseTypes>("DatabaseContainers:AcademicManagement:DatabaseType");
        var academic_hostPort = _configuration.GetValue<int>("DatabaseContainers:AcademicManagement:HostPort");
        var academic_containerPort = _configuration.GetValue<int>("DatabaseContainers:AcademicManagement:ContainerPort");
        var academic_imageName = _configuration.GetValue<string>("DatabaseContainers:AcademicManagement:ImageName");
        var academic_containerName = _configuration.GetValue<string>("DatabaseContainers:AcademicManagement:ContainerName");
        var academic_env_User = _configuration.GetValue<string>("DatabaseContainers:AcademicManagement:Env:USER") ?? throw new ArgumentNullException("USER");
        var academic_env_Password = _configuration.GetValue<string>("DatabaseContainers:AcademicManagement:Env:PASSWORD") ?? throw new ArgumentNullException("PASSWORD");
        var academic_env_Database = _configuration.GetValue<string>("DatabaseContainers:AcademicManagement:Env:DB") ?? throw new ArgumentNullException("DB");

        var financial_buildImage = _configuration.GetValue<bool>("DatabaseContainers:FinancialManagement:BuildImage");
        var financial_databaseType = _configuration.GetValue<DatabaseTypes>("DatabaseContainers:FinancialManagement:DatabaseType");
        var financial_hostPort = _configuration.GetValue<int>("DatabaseContainers:FinancialManagement:HostPort");
        var financial_containerPort = _configuration.GetValue<int>("DatabaseContainers:FinancialManagement:ContainerPort");
        var financial_imageName = _configuration.GetValue<string>("DatabaseContainers:FinancialManagement:ImageName");
        var financial_containerName = _configuration.GetValue<string>("DatabaseContainers:FinancialManagement:ContainerName");
        var financial_env_User = _configuration.GetValue<string>("DatabaseContainers:FinancialManagement:Env:USER") ?? throw new ArgumentNullException("USER");
        var financial_env_Password = _configuration.GetValue<string>("DatabaseContainers:FinancialManagement:Env:PASSWORD") ?? throw new ArgumentNullException("PASSWORD");
        var financial_env_Database = _configuration.GetValue<string>("DatabaseContainers:FinancialManagement:Env:DB") ?? throw new ArgumentNullException("DB");

        // Optionally build the images from the Dockerfiles
        if (academic_buildImage)
        {
            new ImageFromDockerfileBuilder()
                .WithDeleteIfExists(true)
                .WithCleanUp(true)
                .WithDockerfileDirectory(CommonDirectoryPath.GetProjectDirectory(), string.Empty)
                .WithName(academic_imageName)
                .WithDockerfile($"Dockerfile.{academic_databaseType.ToString().ToLower()}")
                .WithCreateParameterModifier(parameter => parameter.Target = "academic_management_db")
                .Build().CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        if (financial_buildImage)
        {
            new ImageFromDockerfileBuilder()
                .WithDeleteIfExists(true)
                .WithCleanUp(true)
                .WithDockerfileDirectory(CommonDirectoryPath.GetProjectDirectory(), string.Empty)
                .WithName(financial_imageName)
                .WithCreateParameterModifier(parameter => parameter.Target = "financial_management_db")
                .WithDockerfile($"Dockerfile.{financial_databaseType.ToString().ToLower()}")
                .Build().CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // Create the academic container
        _academicDatabaseContainer = new ContainerBuilder()
            .WithImage(academic_imageName)
            .WithName(academic_containerName)
            .WithPortBinding(academic_hostPort, academic_containerPort)
            .WithEnvironment(academic_databaseType, academic_env_User, academic_env_Password, academic_env_Database)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(academic_containerPort))
            .Build();

        // Create the financial container
        _financialDatabaseContainer = new ContainerBuilder()
            .WithImage(financial_imageName)
            .WithName(financial_containerName)
            .WithPortBinding(financial_hostPort, financial_containerPort)
            .WithEnvironment(financial_databaseType, financial_env_User, financial_env_Password, financial_env_Database)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(financial_containerPort))
            .Build();

        _services = new ServiceCollection();
        var academicConnectionString = DatabaseFixtureExtensions.PrepareConnectionString(academic_hostPort, academic_env_User, academic_env_Password, academic_env_Database, academic_databaseType);
        var financialConnectionString = DatabaseFixtureExtensions.PrepareConnectionString(financial_hostPort, financial_env_User, financial_env_Password, financial_env_Database, financial_databaseType);


        // create keyed services accorind to the database type
        // TODO: Add the services to the service collection
        _services.AddAcademicCannonizer(academic_databaseType, academicConnectionString);
        _services.AddFinancialCannonizer(financial_databaseType, financialConnectionString);

        _scope = _services.BuildServiceProvider().CreateScope();

        Task.WhenAll(
            _academicDatabaseContainer.StartAsync(),
            _financialDatabaseContainer.StartAsync()
        ).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public T GetService<T>()
        where T : class
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    public T GetService<T>(string key)
        where T : class
    {
        return _scope.ServiceProvider.GetKeyedService<T>(key) ?? throw new InvalidOperationException($"Service with key {key} not found");
    }

    public async void Dispose()
    {
        _scope.Dispose();

        if (_academicDatabaseContainer != null)
            await _academicDatabaseContainer.DisposeAsync();

        if (_financialDatabaseContainer != null)
            await _financialDatabaseContainer.DisposeAsync();
    }
}

internal static class DatabaseFixtureExtensions
{

    internal static IServiceCollection AddAcademicCannonizer(this IServiceCollection services, DatabaseTypes databaseType, string connectionString, bool useAtomicConnection = true)
        => databaseType switch
        {
            DatabaseTypes.MYSQL => services.AddKeyedScoped<ICannonizer, Bifrons.Cannonizers.Relational.Mysql.Cannonizer>("AcademicCannonizer", (_, _) => Cannonizers.Relational.Mysql.Cannonizer.Cons(connectionString, useAtomicConnection).Data ?? throw new Exception("Failed to create cannonizer")),
            DatabaseTypes.POSTGRES => services.AddKeyedScoped<ICannonizer, Bifrons.Cannonizers.Relational.Postgres.Cannonizer>("AcademicCannonizer", (_, _) => Cannonizers.Relational.Postgres.Cannonizer.Cons(connectionString, useAtomicConnection).Data ?? throw new Exception("Failed to create cannonizer")),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };

    internal static IServiceCollection AddFinancialCannonizer(this IServiceCollection services, DatabaseTypes databaseType, string connectionString, bool useAtomicConnection = true)
        => databaseType switch
        {
            DatabaseTypes.MYSQL => services.AddKeyedScoped<ICannonizer, Bifrons.Cannonizers.Relational.Mysql.Cannonizer>("FinancialCannonizer", (_, _) => Cannonizers.Relational.Mysql.Cannonizer.Cons(connectionString, useAtomicConnection).Data ?? throw new Exception("Failed to create cannonizer")),
            DatabaseTypes.POSTGRES => services.AddKeyedScoped<ICannonizer, Bifrons.Cannonizers.Relational.Postgres.Cannonizer>("FinancialCannonizer", (_, _) => Cannonizers.Relational.Postgres.Cannonizer.Cons(connectionString, useAtomicConnection).Data ?? throw new Exception("Failed to create cannonizer")),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };

    internal static string PrepareConnectionString(int port, string user, string password, string database, DatabaseTypes databaseType)
        => databaseType switch
        {
            DatabaseTypes.MYSQL => PrepareMySqlConnectionString(port, user, password, database),
            DatabaseTypes.POSTGRES => PreparePostgresConnectionString(port, user, password, database),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };

    internal static string PrepareMySqlConnectionString(int port, string user, string password, string database)
        => $"Server=localhost;Port={port};Database={database};Uid={user};Pwd={password};";

    internal static string PreparePostgresConnectionString(int port, string user, string password, string database)
        => $"Server=localhost;Port={port};Database={database};Username={user};Password={password};";

    internal static ContainerBuilder WithMysqlEnvironment(this ContainerBuilder builder, string user, string password, string database)
        => builder
            .WithEnvironment("MYSQL_USER", user)
            .WithEnvironment("MYSQL_PASSWORD", password)
            .WithEnvironment("MYSQL_DB", database);

    internal static ContainerBuilder WithPostgresEnvironment(this ContainerBuilder builder, string user, string password, string database)
        => builder
            .WithEnvironment("POSTGRES_USER", user)
            .WithEnvironment("POSTGRES_PASSWORD", password)
            .WithEnvironment("POSTGRES_DB", database);

    internal static ContainerBuilder WithEnvironment(this ContainerBuilder builder, DatabaseTypes databaseType, string user, string password, string database)
        => databaseType switch
        {
            DatabaseTypes.MYSQL => builder.WithMysqlEnvironment(user, password, database),
            DatabaseTypes.POSTGRES => builder.WithPostgresEnvironment(user, password, database),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };
}
