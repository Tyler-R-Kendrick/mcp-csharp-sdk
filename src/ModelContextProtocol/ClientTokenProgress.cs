using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Messages;

namespace ModelContextProtocol;

internal sealed class ClientTokenProgress(IMcpClient client, ProgressToken progressToken)
    : IProgress<ProgressNotificationValue>
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