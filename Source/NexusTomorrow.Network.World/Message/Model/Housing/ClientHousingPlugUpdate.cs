using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingPlugUpdate, Direction.ClientToServer)]
public sealed partial class ClientHousingPlugUpdate
{
    [WireStruct]
    public sealed partial class HousingContribution
    {
        [UInt(32)] public uint ContributionPointRequirement { get; private set; } // derived from tbl housingPlugItem.housingContributionInfoId
        [UInt(32)] public uint Unused1 { get; private set; }
        [UInt(32)] public uint Unused2 { get; private set; }
        [UInt(32)] public uint Unused3 { get; private set; }
        [UInt(32)] public uint Unused4 { get; private set; }
    }

    public Identity ResidenceIdentity { get; private set; } = new();
    [UInt(32)] public uint HousingPlotInfoId { get; private set; }
    [UInt(32)] public uint HousingPlugItemId { get; private set; }
    [UInt(32)] public uint PlugFacing { get; private set; }
    [UInt(32)] public uint PlotFlags { get; private set; } // unused
    [UInt(3)] public HousingPlugOperation Operation { get; private set; }
    [FixedArray(5)] public HousingContribution[] HousingContributions { get; private set; } = new HousingContribution[5];
}
