using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client037E, Direction.ClientToServer)]
public sealed partial class Emote
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
