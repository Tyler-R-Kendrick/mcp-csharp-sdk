using System.Text.Json.Serialization;

namespace ModelContextProtocol.Protocol.Types;

/// <summary>
/// An out-of-band notification used to inform the receiver of a progress update for a long-running request.
/// 
/// <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/2024-11-05/schema.json">See the schema for details</see>
/// </summary>
public class ProgressNotificationParams
{
    /// <summary>
    /// The progress token which was given in the initial request, used to associate this notification with the request that is proceeding.
    /// Progress tokens MUST be a string or integer value
    /// Progress tokens can be chosen by the sender using any means, but MUST be unique across all active requests.
    /// </summary>
    [JsonPropertyName("progressToken")]
    public required object ProgressToken { get; init; }

    /// <summary>
    /// The progress thus far. This should increase every time progress is made, even if the total is unknown.
    /// </summary>
    [JsonPropertyName("progress")]
    public int Progress { get; init; }

    /// <summary>
    /// Total number of items to process (or total progress required), if known.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// An optional message describing the current progress.
    /// Note: this is not specified in the most recent version of the spec, but is included in the most recent documentation for api version 2024-11-05
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}