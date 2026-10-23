using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server08F4, Direction.ServerToClient)]
[LayoutConfirmed]
public sealed partial class Server08F4
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
