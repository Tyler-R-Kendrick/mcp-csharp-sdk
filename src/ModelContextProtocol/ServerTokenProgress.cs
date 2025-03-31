using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Messages;
using ModelContextProtocol.Server;

namespace ModelContextProtocol;

/// <summary>
/// Provides an <see cref="IProgress{ProgressNotificationValue}"/> tied to a specific progress token and that will issue
/// progress notifications to the supplied endpoint.
/// </summary>
internal sealed class TokenProgress(IMcpServer server, ProgressToken progressToken) : IProgress<ProgressNotificationValue>
{
    /// <inheritdoc />
    public void Report(ProgressNotificationValue value)
    {
        _ = server.SendMessageAsync(new JsonRpcNotification()
        {
            Method = NotificationMethods.ProgressNotification,
            Params = new ProgressNotification()
            {
                ProgressToken = progressToken,
                Progress = new()
                {
                    Progress = value.Progress,
                    Total = value.Total,
                    Message = value.Message,
                },
            },
        }, CancellationToken.None);
    }
}

//TODO: Create an intermediary interface to handle common operations (cancel, ping, and progress).
//This will allow a single class to handle this implementation.
internal sealed class ClientTokenProgress(IMcpClient client, ProgressToken progressToken) : IProgress<ProgressNotificationValue>
{
    /// <inheritdoc />
    public void Report(ProgressNotificationValue value)
    {
        _ = client.SendMessageAsync(new JsonRpcNotification()
        {
            Method = NotificationMethods.ProgressNotification,
            Params = new ProgressNotification()
            {
                ProgressToken = progressToken,
                Progress = new()
                {
                    Progress = value.Progress,
                    Total = value.Total,
                    Message = value.Message,
                },
            },
        }, CancellationToken.None);
    }
}