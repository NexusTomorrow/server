using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server067E, Direction.ServerToClient)]
public sealed partial class SoldierWhackAMoleBurrows
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] Unknown1 { get; set; } = [];
}
