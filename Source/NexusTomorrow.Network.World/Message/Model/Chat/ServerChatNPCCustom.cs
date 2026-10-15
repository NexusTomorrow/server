using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatNPCCustom, Direction.ServerToClient)]
public sealed partial class ServerChatNPCCustom
{
    public Channel Channel { get; set; }
    [UInt(21)] public uint UnitNameLocalizedTextId { get; set; }
    [WideString] public string CustomChatText { get; set; }
}
