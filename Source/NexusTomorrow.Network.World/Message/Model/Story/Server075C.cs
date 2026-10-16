using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server075C, Direction.ServerToClient)]
public sealed partial class Server075C
{
    public Server0738Part0 Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
}
