using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingFlagsUpdate, Direction.ClientToServer)]
public sealed partial class ClientHousingFlagsUpdate
{
    public Identity ResidenceIdentity { get; } = new();
    [UInt(32)] public ResidenceFlags Flags { get; private set; }
    [UInt(3)] public byte NeighbourHarvestSplit { get; private set; } // can never be more than 5
    [UInt(3)] public byte NeighbourGardenSplit { get; private set; } // can never be more than 5
}
