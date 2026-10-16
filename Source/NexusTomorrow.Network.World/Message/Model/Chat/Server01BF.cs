using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01BF, Direction.ServerToClient)]
public sealed partial class Server01BF
{
    public Server01B5Part0 Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
}
