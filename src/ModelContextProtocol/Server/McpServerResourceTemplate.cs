using ModelContextProtocol.Protocol.Types;

namespace ModelContextProtocol.Server;

/// <summary>
/// Represents a resource template that the server supports.
/// </summary>
public class McpServerResourceTemplate : IMcpServerPrimitive
{
    /// <summary>
    /// The resource template instance.
    /// </summary>
    public required ResourceTemplate ProtocolResourceTemplate { get; init; }

    /// <inheritdoc />
    public string Name => ProtocolResourceTemplate.Name;
}