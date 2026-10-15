using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingBuildState)]
[WireMessage(GameMessageOpcode.ServerHousingBuildState, Direction.ServerToClient)]
public sealed partial class ServerHousingBuildState : IWritable
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public uint HousingPlotIndex { get; set; }
    [UInt(32)] public uint BuildStage { get; set; } // changes the model sequence for the prop, see tbl housingBuild.modelSequenceId
    [UInt(3)] public BuildState State { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.Write(HousingPlotIndex);
        writer.Write(BuildStage);
        writer.Write(State, 3u);
    }
}
