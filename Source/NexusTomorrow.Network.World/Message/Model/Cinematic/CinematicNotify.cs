using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0232, Direction.ServerToClient)]
public sealed partial class CinematicNotify
{
    [UInt(16)] public ushort Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(14)] public ushort Unknown3 { get; set; }
}
