namespace NexusTomorrow.Gateway.Sts;

/// <summary>
/// One STS request: the request line's method and URI, its headers by name, and its body.
/// </summary>
public sealed record StsRequest(string Method, string Uri, IReadOnlyDictionary<string, string> Headers, string Body);

/// <summary>
/// The outcome of reading one request from a buffer.
/// </summary>
public enum StsReadStatus
{
    /// <summary>
    /// The buffer holds only part of a request.
    /// </summary>
    Incomplete,

    /// <summary>
    /// A whole request was read and consumed.
    /// </summary>
    Complete,

    /// <summary>
    /// The bytes cannot be a request, or exceed a cap; the connection ends.
    /// </summary>
    Malformed,
}
