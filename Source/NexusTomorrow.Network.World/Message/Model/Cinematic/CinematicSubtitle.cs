using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server022A, Direction.ServerToClient)]
public sealed partial class CinematicSubtitle
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(21)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
