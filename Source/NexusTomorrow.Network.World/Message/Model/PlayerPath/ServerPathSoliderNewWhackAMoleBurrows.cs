using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoliderWhackAMoleBurrows, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierWhackAMoleBurrows
{
    [UInt(32)] public uint DelayUntilPop { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIds { get; set; }
}
