using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupLeave, Direction.ClientToServer)]
public sealed partial class ClientGroupLeave
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(1)] public bool ShouldDisband { get; set; }
}
