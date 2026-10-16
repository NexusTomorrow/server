using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireStruct]
public sealed partial class Server0399Part0
{
    [WideString] public string Unknown0 { get; set; } = "";
    public Client00B5Part0 Unknown1 { get; set; } = new();
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(6)] public byte Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(6)] public byte Unknown5 { get; set; }
    [UInt(8)] public byte Unknown6 { get; set; }
    [UInt(24)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(15)] public ushort Unknown9 { get; set; }
    [UInt(14)] public ushort Unknown10 { get; set; }
}

