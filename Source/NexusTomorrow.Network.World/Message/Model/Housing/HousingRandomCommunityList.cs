using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0526, Direction.ServerToClient)]
public sealed partial class HousingRandomCommunityList
{
    [WireStruct]
    public sealed partial class CommunityListing
    {
        public RealmScopedId Unknown0 { get; set; } = new();
        [WideString] public string Unknown1 { get; set; } = "";
        [WideString] public string Unknown2 { get; set; } = "";
    }

    [Array(CountBits = 32)] public List<CommunityListing> Unknown0 { get; set; } = [];
}
