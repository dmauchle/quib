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
    }

    public static readonly string? DOCKER_HOST;
    public static readonly bool CLEANUP_TEST_CONTAINERS;

}
