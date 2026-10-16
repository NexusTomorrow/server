using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(2)]
public sealed partial class Server0738Union0Arm02 : Server0738Union0
{
    [UInt(21)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [NarrowString] public string Unknown2 { get; set; } = "";
}
