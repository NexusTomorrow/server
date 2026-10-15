using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.ServerUnitPvpStateChange, Direction.ServerToClient)]
public sealed partial class ServerUnitPvpStateChange
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public PvpState State { get; set; }
}
