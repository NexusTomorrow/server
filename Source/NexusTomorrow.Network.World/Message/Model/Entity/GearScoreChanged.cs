using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server01A6, Direction.ServerToClient)]
public sealed partial class GearScoreChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
