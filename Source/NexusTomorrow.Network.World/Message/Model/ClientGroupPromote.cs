using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupPromote, Direction.ClientToServer)]
public sealed partial class ClientGroupPromote
{
    [UInt(64)] public ulong GroupId { get; set; }

    public Identity TargetedPlayer { get; set; } = new();
}
