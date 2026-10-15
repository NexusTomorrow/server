using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingPlots)]
[WireMessage(GameMessageOpcode.ServerHousingPlots, Direction.ServerToClient)]
public sealed partial class ServerHousingPlots : IWritable
{
    [WireStruct]
    public sealed partial class Plot : IWritable
    {
        [UInt(32)] public uint PlotPropertyIndex { get; set; }
        [UInt(32)] public uint PlugItemId { get; set; }
        [UInt(32)] public HousingPlugFacing PlugFacing { get; set; }
        [UInt(32)] public uint PlotInfoId { get; set; }
        [FixedArray(5), UInt(32)] public uint[] HousingContributionTotals { get; } = new uint[5];
        [FixedArray(5), UInt(32)] public uint[] HousingDecayTotals { get; } = new uint[5];
        [UInt(32)] public uint HousingUpkeepCharges { get; set; }
        [Float32] public float HousingUpkeepTime { get; set; }
        [UInt(3)] public BuildState BuildState { get; set; }
        [UInt(32)] public uint BuildStage { get; set; } // changes the model's sequenceId
        [Float32] public float BuildBonus { get; set; } // 0.0f to 1.0f, higher is faster build
        [Float32] public float BuildStartTime { get; set; }

        public Plot()
        {
            Array.Fill(HousingContributionTotals, 0u);
            Array.Fill(HousingDecayTotals, 0u);
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PlotPropertyIndex);
            writer.Write(PlugItemId);
            writer.Write(PlugFacing, 32u);
            writer.Write(PlotInfoId);

            for (uint i = 0u; i < HousingContributionTotals.Length; i++)
                writer.Write(HousingContributionTotals[i]);

            for (uint i = 0u; i < HousingDecayTotals.Length; i++)
                writer.Write(HousingDecayTotals[i]);

            writer.Write(HousingUpkeepCharges);
            writer.Write(HousingUpkeepTime);
            writer.Write(BuildState, 3u);
            writer.Write(BuildStage);
            writer.Write(BuildBonus);
            writer.Write(BuildStartTime);
        }
    }

    public Identity ResidenceIdentity { get; set; }
    [Array(CountBits = 32)] public List<Plot> Plots { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);

        writer.Write(Plots.Count);
        Plots.ForEach(p => p.Write(writer));
    }
}
