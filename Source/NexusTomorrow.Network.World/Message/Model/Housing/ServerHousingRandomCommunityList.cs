using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingRandomCommunityList, Direction.ServerToClient)]
public sealed partial class ServerHousingRandomCommunityList
{
    [WireStruct]
    public sealed partial class Community
    {
        public Identity GuildIdentity { get; set; }
        [WideString] public string Name { get; set; }
        [WideString] public string CommunityLeader { get; set; }
    }

    [Array(CountBits = 32)] public List<Community> Communities { get; set; } = [];
}
