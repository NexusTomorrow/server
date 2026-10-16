using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
public sealed partial class Server0738Part0
{
    [WireStruct]
    public sealed partial class Server0738Part1
    {
        [UInt(3)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public Server0738Union0 Unknown1 { get; set; } = new Server0738Union0Arm00();
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountBits = 8)] public List<Server0738Part1> Unknown2 { get; set; } = [];
}
