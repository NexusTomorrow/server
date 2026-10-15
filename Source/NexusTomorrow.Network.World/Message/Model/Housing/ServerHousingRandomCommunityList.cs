using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingRandomCommunityList)]
[WireMessage(GameMessageOpcode.ServerHousingRandomCommunityList, Direction.ServerToClient)]
public sealed partial class ServerHousingRandomCommunityList : IWritable
{
    [WireStruct]
    public sealed partial class Community : IWritable
    {
        public Identity GuildIdentity { get; set; }
        [WideString] public string Name { get; set; }
        [WideString] public string CommunityLeader { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.WriteStringWide(Name);
            writer.WriteStringWide(CommunityLeader);
        }
    }

    [Array(CountBits = 32)] public List<Community> Communities { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Communities.Count);
        Communities.ForEach(c => c.Write(writer));
    }
}
