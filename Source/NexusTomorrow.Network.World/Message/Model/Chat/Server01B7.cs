using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01B7, Direction.ServerToClient)]
public sealed partial class Server01B7
{
    public ChatChannelKey Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
