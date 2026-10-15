using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

/// <remarks>
/// Very similar to <see cref="ServerGenericError"/> except only <see cref="GenericError"/> codes
/// 27, 30, 31, 35, 39, 40, 42, 63, 72, 132 and 156 are handled, anything else defaults to "You can't use that"
/// </remarks>
[WireMessage(GameMessageOpcode.ServerItemError, Direction.ServerToClient)]
public sealed partial class ServerItemError
{
    [UInt(64)] public ulong ItemGuid { get; set; }
    [UInt(32)] public GenericError ErrorCode { get; set; }
    [Pad(32)] public Pad ErrorCodeHigh { get; }
}
