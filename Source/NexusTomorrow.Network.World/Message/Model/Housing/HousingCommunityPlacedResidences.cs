using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server04DD, Direction.ServerToClient)]
public sealed partial class HousingCommunityPlacedResidences
{
    [WireStruct]
    public sealed partial class PlacedResidence
    {
        public RealmScopedId Unknown0 { get; set; } = new();
        [WideString] public string Unknown1 { get; set; } = "";
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<PlacedResidence> Unknown0 { get; set; } = [];
}
