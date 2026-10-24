using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server01B4, Direction.ServerToClient)]
public sealed partial class ZoneMapExplored
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Blob(32)] public byte[] Unknown1 { get; set; } = [];
}
