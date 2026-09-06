using System.Data.Common;

namespace Quib.UnitTests.Infrastructure;

[CollectionDefinition(TestCollections.DATABASE)]
public class DatabaseTestCollection : ICollectionFixture<DatabaseFixture>
{
}

/// <summary>
/// Provides a Postgres database instance on Docker.
/// </summary>
public class DatabaseFixture : IAsyncLifetime
{

    private readonly PostgresContainer _dbContainer = new PostgresContainer();

    public async Task InitializeAsync()
    {
        await _dbContainer.InitializeAsync();
        await DatabaseReady();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private async Task DatabaseReady()
    {
        var start = DateTime.Now;
        while (true)
        {
            try
            {
                var con = await OpenConnection();
                con.Dispose();
                return;
            }
            catch (DbException)
            {
                // db container might not yet be ready
                if (DateTime.Now.Subtract(start).TotalMinutes > 1)
                    throw;
                else
                    await Task.Delay(2000);
            }
        }
    }

    public async Task<DbConnection> OpenConnection()
    {
        //var pgBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbContainer.ConnectionString);
        var con = new Npgsql.NpgsqlConnection(_dbContainer.ConnectionString);
        await con.OpenAsync();
        return con;
    }

    public async Task ResetDatabase()
    {
        // Determine target database name from the connection string
        var pgBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbContainer.ConnectionString);
        var targetDb = pgBuilder.Database;

        // Connect explicitly to the maintenance DB (postgres) to run DROP/CREATE database commands
        pgBuilder.Database = "postgres";

        // Clear local connection pools before attempting to terminate backends/drop the database.
        // Pooled connections in this process can keep server-side backends alive and block DROP.
        Npgsql.NpgsqlConnection.ClearAllPools();

        using (var con = new Npgsql.NpgsqlConnection(pgBuilder.ConnectionString))
        {
            await con.OpenAsync();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"DROP DATABASE IF EXISTS \"{targetDb}\";";
                await cmd.ExecuteNonQueryAsync();
            }
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"CREATE DATABASE \"{targetDb}\";";
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}