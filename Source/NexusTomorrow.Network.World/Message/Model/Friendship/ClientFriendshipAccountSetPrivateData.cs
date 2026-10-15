using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPrivateData, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPrivateData
{
    [UInt(64)] public ulong AccountFriendId { get; private set; }
    [UInt(32)] public uint Unused { get; private set; } // Always 2
    [WideString] public string PrivateDataText { get; private set; }
}
