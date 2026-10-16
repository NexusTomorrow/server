using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(1)]
public sealed partial class Server0738Union0Arm01 : Server0738Union0
{
    [WideString] public string Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [NarrowString] public string Unknown2 { get; set; }
}
