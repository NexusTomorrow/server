#if DEBUG
using System.Buffers;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Gateway.Connections;

/// <summary>
/// A connection's Debug-only surface for the oracle's tools: sends that skip the send gate, and a tap on every message the connection
/// reads or writes. None of it exists in a Release build.
/// </summary>
public interface IDebugSession
{
    /// <summary>
    /// The connection's id, as its log lines name it.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// The tap the connection reports every message to, or null for none. Set from any thread.
    /// </summary>
    ISessionTap? Tap { get; set; }

    /// <summary>
    /// Queue <paramref name="message"/> under <paramref name="opcode"/> without asking the send gate
    /// (<see cref="MessageCatalog.MaySend"/>). The message is serialized before this returns.
    /// </summary>
    void SendUnchecked(ushort opcode, IWireStruct message);

    /// <summary>
    /// Queue <paramref name="body"/> under <paramref name="opcode"/> exactly as given, for input no declaration can build.
    /// </summary>
    void SendRaw(ushort opcode, ReadOnlySpan<byte> body);
}

/// <summary>
/// Receives every message a connection reads or writes, called on the connection's read loop and writer. It must return quickly; a
/// tap that throws is detached.
/// </summary>
public interface ISessionTap
{
    /// <summary>
    /// A frame the connection read: the clear envelope as the decoder left it, and what the decoder made of it.
    /// </summary>
    void Received(long session, ReadOnlySpan<byte> envelope, Inbound inbound);

    /// <summary>
    /// A message the connection wrote: its opcode, its clear body, and the type it was built from (<see cref="InjectedBody"/> for a body
    /// queued by <see cref="IDebugSession.SendRaw"/>).
    /// </summary>
    void Sent(long session, ushort opcode, ReadOnlySpan<byte> body, Type messageType);
}

/// <summary>
/// Stands where a sent message's type would be for a body queued by <see cref="IDebugSession.SendRaw"/>.
/// </summary>
public sealed class InjectedBody;

/// <content>
/// The Debug-only surface (<see cref="IDebugSession"/>).
/// </content>
public abstract partial class ClientConnection : IDebugSession
{
    private ISessionTap? _tap;

    /// <inheritdoc/>
    public ISessionTap? Tap
    {
        get => Volatile.Read(ref _tap);
        set => Volatile.Write(ref _tap, value);
    }

    /// <inheritdoc/>
    public void SendUnchecked(ushort opcode, IWireStruct message) => Enqueue(Outbound.Enveloped(opcode, message));

    /// <inheritdoc/>
    public void SendRaw(ushort opcode, ReadOnlySpan<byte> body) => Enqueue(Outbound.Raw(opcode, body));

    private void TapReceived(byte[] envelope, in Inbound inbound)
    {
        if (Tap is not { } tap)
            return;

        try
        {
            tap.Received(Id, envelope, inbound);
        }
        catch (Exception exception)
        {
            Detach(tap, exception);
        }
    }

    private void TapSent(in Outbound item)
    {
        if (item.Kind is not (OutboundKind.Bare or OutboundKind.Enveloped) || Tap is not { } tap)
            return;

        try
        {
            // Body and MessageType are set by every factory that makes a Bare or Enveloped item.
            tap.Sent(Id, item.Opcode, item.Body.AsSpan(0, item.BodyLength), item.MessageType!);
        }
        catch (Exception exception)
        {
            Detach(tap, exception);
        }
    }

    // A faulty tap never ends the connection it watches.
    private void Detach(ISessionTap tap, Exception exception)
    {
        Interlocked.CompareExchange(ref _tap, null, tap);
        DebugLog.TapDetached(_logger, Id, exception);
    }
}

/// <content>
/// A body queued as given (<see cref="IDebugSession.SendRaw"/>).
/// </content>
internal readonly partial record struct Outbound
{
    public static Outbound Raw(ushort opcode, ReadOnlySpan<byte> body)
    {
        byte[] rented = ArrayPool<byte>.Shared.Rent(Math.Max(body.Length, 1));
        body.CopyTo(rented);
        return new Outbound(OutboundKind.Enveloped, opcode, rented, body.Length, typeof(InjectedBody), null, 0);
    }
}

internal static partial class DebugLog
{
    [LoggerMessage(EventId = 4012, Level = LogLevel.Warning, Message = "Connection {ConnectionId}'s tap threw and was detached")]
    public static partial void TapDetached(ILogger logger, long connectionId, Exception exception);
}
#endif
