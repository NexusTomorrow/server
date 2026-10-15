using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerTitleUpdate, Direction.ServerToClient)]
public sealed partial class ServerTitleUpdate
{
    [UInt(14)] public ushort TitleId { get; set; }
    [UInt(1)] public bool Alreadyowned { get; set; }
    [UInt(1)] public bool Revoked { get; set; }
}
