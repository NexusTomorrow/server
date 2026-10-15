namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Which way a message travels between the client and the server.
/// </summary>
public enum Direction
{
    /// <summary>
    /// The client sends it and the server reads it.
    /// </summary>
    ClientToServer = 1,

    /// <summary>
    /// The server sends it and the client reads it.
    /// </summary>
    ServerToClient = 2,

    /// <summary>
    /// Either side sends it.
    /// </summary>
    Both = 3,
}
