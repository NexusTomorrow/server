using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireStruct]
public sealed partial class FriendData : IWritable
{
    [UInt(64)] public ulong FriendshipId { get; set; }
    public Identity PlayerIdentity { get; set; } = new();
    [WideString] public string Note { get; set; } = "";
    [UInt(4)] public FriendshipType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FriendshipId);
        PlayerIdentity.Write(writer);
        writer.WriteStringWide(Note);
        writer.Write(Type, 4u);
    }
}
