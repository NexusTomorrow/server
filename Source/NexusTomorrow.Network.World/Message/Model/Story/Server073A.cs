using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server073A, Direction.ServerToClient)]
public sealed partial class Server073A
{
    public Server0738Part0 Unknown0 { get; set; } = new();
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(2)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
}
