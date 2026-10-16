using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01B7, Direction.ServerToClient)]
public sealed partial class Server01B7
{
    public Server01B5Part0 Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
