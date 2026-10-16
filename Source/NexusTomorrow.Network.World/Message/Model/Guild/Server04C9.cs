using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04C9, Direction.ServerToClient)]
public partial class Server04C9
{
    public Client00B5Part0 Unknown0 { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [UInt(8)] public byte Unknown3 { get; set; }
}
