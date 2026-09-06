using Docker.DotNet.Models;

namespace Quib.UnitTests.Infrastructure;

public class PostgresContainer : IAsyncLifetime
{

    private static string SelectContainerImage()
    {
        // Pin to a specific Postgres version. Might be changed to a version selector like in MySqlContainer.
        return "postgres:18.6";
    }


    private readonly string _dbContainerName = "quib_postgres_tests";
    private readonly string _dbContainerImage = SelectContainerImage();


    public PostgresContainer()
    {
        this.DbHost = EnvironmentVariables.DOCKER_HOST ?? "127.0.0.1";
        // Map container default Postgres port (5432) to a host port (kept as 18424 to avoid conflicts)
        this.DbPort = 18424;
        this.DbName = "quib";
        this.DbUser = "postgres";
        this.DbPassword = "postgres";
        this.Charset = string.Empty;
        this.ConnectionString = $"Host={DbHost};Port={DbPort};Database={DbName};Username={DbUser};Password={DbPassword}";
    }


    public async Task InitializeAsync()
    {

        Console.WriteLine($"Providing postgres database on host machine @ {this.DbHost}");

        using (var docker = new DockerController())
        {

            var existingContainer = await docker.GetExistingContainer(_dbContainerName);

            if (existingContainer != null)
            {
                // If the image changed, remove the old container so we recreate it.
                if (existingContainer.Image != _dbContainerImage)
                {
                    await docker.Containers.RemoveContainerAsync(existingContainer.ID, new ContainerRemoveParameters { Force = true });
                    existingContainer = null;
                }
            }

            if (existingContainer == null)
            {
                await docker.LoadImage(_dbContainerImage);

                var postgres = new CreateContainerParameters
                {
                    Image = _dbContainerImage,
                    Name = _dbContainerName,
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            { "5432", [new PortBinding { HostIP = "0.0.0.0", HostPort = this.DbPort.ToString() }] }
                        }
                    },
                    Env =
                    [
                        $"POSTGRES_PASSWORD={this.DbPassword}",
                        $"POSTGRES_USER={this.DbUser}",
                        $"POSTGRES_DB={this.DbName}",
                    ],
                    ExposedPorts = new Dictionary<string, EmptyStruct>
                    {
                        { "5432", new EmptyStruct() }
                    }
                };

                var newContainer = await docker.Containers.CreateContainerAsync(postgres);
                await docker.Containers.StartContainerAsync(newContainer.ID, new ContainerStartParameters());
            }
            else
            {
                if (existingContainer.State != "running")
                    await docker.Containers.StartContainerAsync(existingContainer.ID, new ContainerStartParameters());
            }
        }
    }

    public async Task DisposeAsync()
    {
        using (var docker = new DockerController())
        {
            if (EnvironmentVariables.CLEANUP_TEST_CONTAINERS)
            {
                var existingContainer = await docker.GetExistingContainer(_dbContainerName);
                if (existingContainer != null)
                {
                    await docker.Containers.RemoveContainerAsync(existingContainer.ID, new ContainerRemoveParameters { Force = true });
                }
            }
        }
    }

    public string DbHost { get; private set; }
    public int DbPort { get; private set; }
    public string DbName { get; private set; }
    public string DbUser { get; private set; }
    public string DbPassword { get; private set; }

    public string Charset { get; private set; }

    public string ConnectionString { get; private set; }

}