using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client07B6, Direction.ClientToServer)]
public sealed partial class Client07B6
{
    [WireStruct]
    public sealed partial class Client07B6Part0
    {
        [UInt(4)] public byte Unknown0 { get; set; }
        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(8)] public byte Unknown2 { get; set; }
        [WideString] public string Unknown3 { get; set; } = "";
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [Array(CountBits = 32)] public List<Client07B6Part0> Unknown4 { get; } = [];
}
