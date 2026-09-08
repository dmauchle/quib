using Docker.DotNet.Models;

namespace Quib.UnitTests.Infrastructure;


public class MySqlContainer : IAsyncLifetime
{

    public static MySqlContainer CreateV56() => new MySqlContainer(dockerVersion: "5.6", versionCode: 56);
    public static MySqlContainer CreateV57() => new MySqlContainer(dockerVersion: "5.7", versionCode: 57);
    public static MySqlContainer CreateV80() => new MySqlContainer(dockerVersion: "8.0", versionCode: 80);
    public static MySqlContainer CreateV84() => new MySqlContainer(dockerVersion: "8.4", versionCode: 84);


    private readonly string _dbContainerName;
    private readonly string _dbContainerImage;
    private readonly string[] _dbContainerCommands;


    private MySqlContainer(string dockerVersion, int versionCode)
    {
        _dbContainerName = $"quib_mysql{versionCode}_tests";
        _dbContainerImage = $"mysql:{dockerVersion}";

        _dbContainerCommands = [
            "--sql_mode=ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION",
            "--max-allowed-packet=30000000"
        ];

        this.DbHost = EnvironmentVariables.DOCKER_HOST ?? "127.0.0.1";
        this.DbPort = 18400 + versionCode;
        this.DbName = "quib";
        this.DbUser = "root";
        this.DbPassword = "root";
        this.Charset = "utf8mb4";
        this.ConnectionString = $"Server={DbHost};Port={DbPort};Database={DbName};Username={DbUser};Password={DbPassword};CharSet={Charset}";
    }


    public async Task InitializeAsync()
    {

        Console.WriteLine($"Providing mysql database on host machine @ {this.DbHost}");

        using (var docker = new DockerController())
        {

            var existingContainer = await docker.GetExistingContainer(_dbContainerName);

            if (existingContainer != null)
            {
                if (existingContainer.Image != _dbContainerImage || existingContainer.Command != $"docker-entrypoint.sh {string.Join(" ", _dbContainerCommands)}")
                {
                    await docker.Containers.RemoveContainerAsync(existingContainer.ID, new ContainerRemoveParameters { Force = true });
                    existingContainer = null;
                }
            }

            if (existingContainer == null)
            {
                await docker.LoadImage(_dbContainerImage);

                var mysql = new CreateContainerParameters
                {
                    Image = _dbContainerImage,
                    Name = _dbContainerName,
                    Cmd = _dbContainerCommands,
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            { "3306", [ new PortBinding { HostIP = "0.0.0.0", HostPort = this.DbPort.ToString() } ] }
                        }
                    },
                    Env = [
                        $"MYSQL_ROOT_PASSWORD={this.DbPassword}",
                        $"MYSQL_DATABASE={this.DbName}"
                    ],
                    ExposedPorts = new Dictionary<string, EmptyStruct>
                    {
                        { "3306", new EmptyStruct() }
                    }
                };

                var newContainer = await docker.Containers.CreateContainerAsync(mysql);
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