using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01CA, Direction.ServerToClient)]
public sealed partial class Server01CA
{
    public Server01B5Part0 Unknown0 { get; set; } = new();
    [UInt(21)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
