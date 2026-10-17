using System.Net;
using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Gateway;

// Nothing here takes a key, salt, verifier, token, password or login name: a log line must stay safe to share.
internal static partial class GatewayLog
{
    [LoggerMessage(EventId = 4001, Level = LogLevel.Information, Message = "STS connection from {Remote} ended: {Reason}")]
    public static partial void StsConnectionEnded(ILogger logger, IPEndPoint remote, string reason);

    [LoggerMessage(EventId = 4002, Level = LogLevel.Warning, Message = "STS request {Uri} refused in state {State}")]
    public static partial void StsRequestRefused(ILogger logger, string uri, string state);

    [LoggerMessage(EventId = 4003, Level = LogLevel.Error, Message = "An STS connection failed unexpectedly")]
    public static partial void StsFailed(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 4004, Level = LogLevel.Information, Message = "Connection {ConnectionId} ended: {Reason}")]
    public static partial void ConnectionEnded(ILogger logger, long connectionId, string reason);

    [LoggerMessage(EventId = 4005, Level = LogLevel.Error, Message = "Connection {ConnectionId} failed unexpectedly")]
    public static partial void ConnectionFailed(ILogger logger, long connectionId, Exception exception);

    [LoggerMessage(EventId = 4006, Level = LogLevel.Debug, Message = "Connection {ConnectionId} dropped message 0x{Opcode:X4}: {Reason}")]
    public static partial void MessageDropped(ILogger logger, long connectionId, ushort opcode, string reason);
}
