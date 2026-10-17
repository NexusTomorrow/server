using System.Net;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Owners;

/// <summary>
/// A client connection as an owner sees it: something to send messages to and to disconnect. Every member is safe to call from any
/// thread.
/// </summary>
public interface IClientSession
{
    /// <summary>
    /// The connection's id, unique for the process's lifetime.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// The client's address.
    /// </summary>
    IPAddress RemoteAddress { get; }

    /// <summary>
    /// Queue <paramref name="message"/> for the client. The message must not be changed after this call.
    /// </summary>
    void Send<T>(T message)
        where T : class, IWireMessage;

    /// <summary>
    /// End the connection after the messages already queued, logging <paramref name="reason"/>.
    /// </summary>
    void Disconnect(string reason);
}
