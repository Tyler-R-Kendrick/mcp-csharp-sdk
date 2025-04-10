using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Messages;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;

namespace ModelContextProtocol.Tests.Configuration;

public class McpServerBuilderExtensionsResourceTemplatesTests(ITestOutputHelper testOutputHelper)
    : ClientServerTestBase(testOutputHelper)
{
    protected override void ConfigureServices(ServiceCollection services, IMcpServerBuilder mcpServerBuilder)
    {
        mcpServerBuilder = mcpServerBuilder.WithResourceTemplates(
            new ResourceTemplate
            {
                Name = "test",
                Description = "test resource",
                UriTemplate = "test.txt",
            },
            new ResourceTemplate
            {
                Name = "test2",
                Description = "test2 resource",
                UriTemplate = "test2.txt",
            });
        base.ConfigureServices(services, mcpServerBuilder);
    }

    [Fact]
    public void Adds_ResourceTemplates_To_Server()
    {        
        var serverOptions = ServiceProvider.GetRequiredService<IOptions<McpServerOptions>>().Value;
        var resources = serverOptions?.Capabilities?.Resources?.ResourceTemplateCollection;
        Assert.NotNull(resources);
        Assert.Equal(2, resources.Count);
        Assert.Equal("test", resources["test"].Name);
        Assert.Equal("test2", resources["test2"].Name);
    }

    [Fact]
    public async Task Can_List_ResourceTemplates()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var client = await CreateMcpClientForServer();

        // Act
        var resources = await client.ListResourcesAsync(token);

        // Assert
        Assert.NotNull(resources);
        Assert.Equal(2, resources.Count);
    }

    [Fact]
    public async Task Can_Be_Notified_Of_ResourceList_Changes()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var client = await CreateMcpClientForServer();
        var serverOptions = ServiceProvider
            .GetRequiredService<IOptions<McpServerOptions>>()
            .Value;
        TaskCompletionSource<JsonRpcNotification> changeReceived = new();
        await using var _ = client.RegisterNotificationHandler(
            NotificationMethods.ResourceListChangedNotification,
            (notification, token) =>
            {
                changeReceived.SetResult(notification);
                return changeReceived.Task;
            });

        // Act
        var resources = await client.ListResourcesAsync(token);
        Assert.NotNull(resources);
        Assert.Equal(2, resources.Count);

        serverOptions?.Capabilities?.Resources?.ResourceTemplateCollection?.Add(new McpServerResourceTemplate
        {
            ProtocolResourceTemplate = new()
            {
                Name = "new resource",
                UriTemplate = "test3.txt",
            },
        });

        // Assert
        await changeReceived.Task.WaitAsync(TimeSpan.FromSeconds(3), token);
        var updatedResources = await client.ListResourcesAsync(token);
        Assert.NotNull(updatedResources);
        Assert.Equal(3, updatedResources.Count);
    }
}
