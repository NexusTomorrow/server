using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B9, Direction.ServerToClient)]
public sealed partial class ExplorerClueUnitsAdded
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] Unknown1 { get; set; } = [];
}
