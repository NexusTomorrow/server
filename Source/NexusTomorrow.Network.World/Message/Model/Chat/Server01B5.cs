using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01B5, Direction.ServerToClient)]
public sealed partial class Server01B5
{
    public Server01B5Part0 Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
}
