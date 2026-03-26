using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using Weather.Persistence;
using Weather.Services;

namespace Weather.Tests.Core;

public abstract class BaseDatabaseTest : IDisposable
{
    private readonly string TestDatabaseName = $"weather_test_{Guid.NewGuid():N}";
    private IDbContextFactory<WeatherDatabaseContext> contextFactory;
    private readonly ConfigurationService configurationService;
    private bool disposed;

    protected BaseDatabaseTest()
    {
        configurationService = new ConfigurationService();
        _ = configurationService.BuildConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<WeatherDatabaseContext>();

        var connectionString = ReplaceDatabase(configurationService.GetConnectionString(), TestDatabaseName);

        configurationService.ConfigureDatabaseOptions(optionsBuilder, connectionString);

        contextFactory = new PooledDbContextFactory<WeatherDatabaseContext>(optionsBuilder.Options);

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    protected WeatherDatabaseContext CreateContext()
    {
        return contextFactory.CreateDbContext();
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        using var context = CreateContext();
        context.Database.EnsureDeleted();

        disposed = true;
        GC.SuppressFinalize(this);
    }

    private string ReplaceDatabase(string connectionString, string testDatabaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = testDatabaseName
        };

        return builder.ConnectionString;
    }
}
