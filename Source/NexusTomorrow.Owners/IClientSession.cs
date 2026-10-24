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
    /// Queue <paramref name="message"/> for the client. The message is serialized before this returns, so the caller may
    /// change or reuse it at once; the send gate runs on the message as it is at the call. Throws <see cref="InvalidOperationException"/>
    /// when the send gate refuses the message: its type is not read to Fields, nor to Purpose with only verified members set, and
    /// <see cref="CodecException"/> when the message wrote a different number of bytes than it measured.
    /// </summary>
    void Send<T>(T message)
        where T : class, IWireMessage;

    /// <summary>
    /// Queue a message already serialized for many sessions (<see cref="IMessageSerializer"/>). The session takes a reference of its
    /// own and releases it once the message is written or dropped, so the caller still releases its own. The send gate ran when the
    /// message was serialized.
    /// </summary>
    void Send(SerializedMessage message);

    /// <summary>
    /// End the connection after the messages already queued, logging <paramref name="reason"/>.
    /// </summary>
    void Disconnect(string reason);
}
