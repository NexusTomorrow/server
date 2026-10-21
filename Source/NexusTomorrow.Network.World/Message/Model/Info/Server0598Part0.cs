using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireStruct]
public sealed partial class Server0598Part0
{
    [UInt(3)] public byte Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new();
    [TerminatedWideString] public string Unknown2 { get; set; } = "";
    [UInt(14)] public ushort Unknown3 { get; set; }
}
