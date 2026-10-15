using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierBuildCredits, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierBuildCredits
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint BuildCredits { get; set; }
}
