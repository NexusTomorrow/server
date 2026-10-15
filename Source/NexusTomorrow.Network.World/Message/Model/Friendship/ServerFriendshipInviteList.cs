using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ServerFriendshipInviteList)]
[WireMessage(GameMessageOpcode.ServerFriendshipInviteList, Direction.ServerToClient)]
public sealed partial class ServerFriendshipInviteList : IWritable
{
    [WireStruct]
    public sealed partial class InviteData : IWritable
    {
        [UInt(64)] public ulong InviteId { get; set; }
        public Identity PlayerIdentity { get; set; }
        [UInt(3)] public uint Seen { get; set; }
        [Float32] public float ExpiryInDays { get; set; }
        [WideString] public string Note { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(8)] public Class Class { get; set; }
        [Pad(6)] public Pad ClassHigh { get; }
        [UInt(3)] public Game.Static.PlayerPath.Path Path { get; set; }
        [UInt(8)] public byte Level { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(InviteId);
            PlayerIdentity.Write(writer);
            writer.Write(Seen, 3u);
            writer.Write(ExpiryInDays);
            writer.WriteStringWide(Note);
            writer.WriteStringWide(Name);
            writer.Write(Class, 14u);
            writer.Write(Path, 3u);
            writer.Write(Level);
        }
    }

    [Array(CountBits = 16)] public List<InviteData> Invites { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Invites.Count, 16u);
        Invites.ForEach(f => f.Write(writer));
    }
}
