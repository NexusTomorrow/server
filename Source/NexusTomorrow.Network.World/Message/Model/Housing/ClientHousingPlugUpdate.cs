using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingPlugUpdate)]
[WireMessage(GameMessageOpcode.ClientHousingPlugUpdate, Direction.ClientToServer)]
public sealed partial class ClientHousingPlugUpdate : IReadable
{
    [WireStruct]
    public sealed partial class HousingContribution : IReadable
    {
        [UInt(32)] public uint ContributionPointRequirement { get; private set; } // derived from tbl housingPlugItem.housingContributionInfoId
        [UInt(32)] public uint Unused1 { get; private set; }
        [UInt(32)] public uint Unused2 { get; private set; }
        [UInt(32)] public uint Unused3 { get; private set; }
        [UInt(32)] public uint Unused4 { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ContributionPointRequirement = reader.ReadUInt();
            Unused1 = reader.ReadUInt();
            Unused2 = reader.ReadUInt();
            Unused3 = reader.ReadUInt();
            Unused4 = reader.ReadUInt();
        }
    }

    public Identity ResidenceIdentity { get; private set; } = new();
    [UInt(32)] public uint HousingPlotInfoId { get; private set; }
    [UInt(32)] public uint HousingPlugItemId { get; private set; }
    [UInt(32)] public uint PlugFacing { get; private set; }
    [UInt(32)] public uint PlotFlags { get; private set; } // unused
    [UInt(3)] public HousingPlugOperation Operation { get; private set; }
    [FixedArray(5)] public HousingContribution[] HousingContributions { get; private set; } = new HousingContribution[5];

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);

        HousingPlotInfoId = reader.ReadUInt();
        HousingPlugItemId = reader.ReadUInt();
        PlugFacing = reader.ReadUInt();
        PlotFlags = reader.ReadUInt();
        Operation = reader.ReadEnum<HousingPlugOperation>(3u);

        for (uint i = 0; i < HousingContributions.Length; i++)
        {
            HousingContributions[i] = new HousingContribution();
            HousingContributions[i].Read(reader);
        }
    }
}
