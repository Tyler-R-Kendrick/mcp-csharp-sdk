namespace ModelContextProtocol.Protocol.Types;

/// <summary>
/// Sent from the client to request a list of resources the server has.
/// <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/2024-11-05/schema.json">See the schema for details</see>
/// </summary>
public class ListResourcesRequestParams : PaginatedRequestParams;
