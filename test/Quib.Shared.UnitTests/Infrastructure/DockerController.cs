using Docker.DotNet;
using Docker.DotNet.Models;
using System.Runtime.InteropServices;

namespace Quib.UnitTests.Infrastructure;

internal class DockerController : IDisposable
{

    private readonly DockerClientConfiguration _dockerConfig;
    private readonly DockerClient _docker;

    public DockerController()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            _dockerConfig = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
        else
            _dockerConfig = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"));
        _docker = _dockerConfig.CreateClient();
    }

    public IContainerOperations Containers => _docker.Containers;

    public async Task LoadImage(string imageName)
    {
        // Check if the image already exists locally
        var images = await _docker.Images.ListImagesAsync(new ImagesListParameters
        {
            All = true
        });

        var imageExists = images.Any(i => i.RepoTags != null && i.RepoTags.Contains(imageName));

        if (!imageExists)
        {
            // Pull the image from the registry
            await _docker.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = imageName
                },
                null,
                new Progress<JSONMessage>());
        }
    }

    public async Task<ContainerListResponse?> GetExistingContainer(string containerName)
    {
        var allContainers = await _docker.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
        var existingContainer = allContainers.FirstOrDefault(x => x.Names.Contains($"/{containerName}"));
        return existingContainer;
    }

    public void Dispose()
    {
        _docker.Dispose();
        _dockerConfig.Dispose();
    }

}