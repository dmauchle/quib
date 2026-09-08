using System.Data.Common;
using MySqlClient = MySql.Data.MySqlClient;

namespace Quib.UnitTests.Infrastructure;

/// <summary>
/// Provides a MySQL database instance on Docker.
/// </summary>
public abstract class MySqlFixture : IAsyncLifetime
{

    private readonly MySqlContainer _container;

    protected MySqlFixture(MySqlContainer container)
    {
        _container = container;
    }

    public async Task InitializeAsync()
    {
        await _container.InitializeAsync();
        await DatabaseReady();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
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
        var con = new MySqlClient.MySqlConnection(_container.ConnectionString);
        await con.OpenAsync();
        return con;
    }

    public async Task ResetDatabase()
    {
        using (var con = await this.OpenConnection())
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"drop database if exists {_container.DbName}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"create database {_container.DbName}";
                cmd.ExecuteNonQuery();
            }
        }
    }

}
