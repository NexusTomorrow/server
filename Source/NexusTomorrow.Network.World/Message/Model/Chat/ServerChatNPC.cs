using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatNPC, Direction.ServerToClient)]
public sealed partial class ServerChatNPC
{
    public Channel Channel { get; set; }
    [UInt(21)] public uint UnitNameLocalizedTextId { get; set; }
    [UInt(21)] public uint MessageLocalizedTextId { get; set; }
}
