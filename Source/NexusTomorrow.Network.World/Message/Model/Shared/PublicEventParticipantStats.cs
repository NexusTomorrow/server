using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server00D6Part2
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    public Client00B5Part0 Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    [UInt(24)] public uint Unknown4 { get; set; }
    [UInt(8)] public byte Unknown5 { get; set; }
    [UInt(24)] public uint Unknown6 { get; set; }
    public Server00D6Part0 Unknown7 { get; set; }
}
