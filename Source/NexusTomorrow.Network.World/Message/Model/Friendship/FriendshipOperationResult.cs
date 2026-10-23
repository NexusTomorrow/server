using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C4, Direction.ServerToClient)]
public sealed partial class FriendshipOperationResult
{
    [WideString] public string Unknown0 { get; set; } = "";
    [UInt(6)] public byte Unknown1 { get; set; }
}
