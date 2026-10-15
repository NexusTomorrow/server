using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountSetPrivateData)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPrivateData, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPrivateData : IReadable
{
    [UInt(64)] public ulong AccountFriendId { get; private set; }
    [UInt(32)] public uint Unused { get; private set; } // Always 2
    [WideString] public string PrivateDataText { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AccountFriendId = reader.ReadULong();
        Unused = reader.ReadUInt();
        PrivateDataText = reader.ReadWideString();
    }
}
