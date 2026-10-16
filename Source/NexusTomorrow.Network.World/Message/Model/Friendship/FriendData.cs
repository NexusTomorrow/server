using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireStruct]
public sealed partial class Server03B5Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; } = new();
    [WideString] public string Unknown2 { get; set; } = "";
    [UInt(4)] public byte Unknown3 { get; set; }
}
