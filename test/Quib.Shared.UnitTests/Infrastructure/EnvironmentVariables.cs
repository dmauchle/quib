namespace Quib.UnitTests.Infrastructure;

internal class EnvironmentVariables
{

    static EnvironmentVariables()
    {
        DOCKER_HOST = Environment.GetEnvironmentVariable("DOCKER_HOST");

        var cleanup = Environment.GetEnvironmentVariable("CLEANUP_TEST_CONTAINERS");
        if (cleanup == "1")
            CLEANUP_TEST_CONTAINERS = true;
        else if (cleanup == "0")
            CLEANUP_TEST_CONTAINERS = false;
        else if (cleanup != null)
            throw new NotSupportedException($"Environment variable CLEANUP_TEST_CONTAINERS value '{cleanup}' not supported. Use '1' or '0'.");

        var version = Environment.GetEnvironmentVariable("MYSQL_VERSION");
        if (version == "8")
            MYSQL_VERSION = MysqlVersion.V8;
        else if (version == "5.7")
            MYSQL_VERSION = MysqlVersion.V57;
        else if (version != null)
            throw new NotSupportedException($"Environment variable MYSQL_VERSION value '{version}' not supported. Use '8' or '5.7'.");
    }

    public static readonly string? DOCKER_HOST;
    public static readonly bool CLEANUP_TEST_CONTAINERS;
    public static readonly MysqlVersion MYSQL_VERSION;


    public enum MysqlVersion { V8, V57 };

}
