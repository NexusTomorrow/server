using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(7)]
public sealed partial class Client0959Union0Arm07 : IClient0959Union0
{
    [WideString] public string Unknown0 { get; private set; }
    [UInt(8)] public byte Unknown1 { get; private set; }
    [UInt(6)] public byte Unknown2 { get; set; }
    [UInt(3)] public byte Unknown3 { get; private set; }
    [UInt(8)] public byte Unknown4 { get; private set; }
    [UInt(6)] public byte Unknown5 { get; set; }
    [UInt(15)] public ushort Unknown6 { get; private set; }
    [UInt(14)] public ushort Unknown7 { get; private set; }
}
