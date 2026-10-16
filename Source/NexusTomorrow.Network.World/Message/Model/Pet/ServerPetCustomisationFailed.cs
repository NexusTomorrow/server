using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server068B, Direction.ServerToClient)]
public sealed partial class Server068B
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
    [UInt(14)] public ushort Unknown4 { get; set; }
}
