using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

// TODO:: confirm
[WireMessage(GameMessageOpcode.ServerCinematicAdvanceToEnding, Direction.ServerToClient)]
public sealed partial class ServerCinematicAdvanceToEnding
{
    [UInt(8)] public byte Unused { get; set; }
}
