using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01DA, Direction.ServerToClient)]
public sealed partial class Server01DA
{
    [UInt(15)] public ushort Unknown0 { get; set; }
}
