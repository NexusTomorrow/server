using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingBuildState, Direction.ServerToClient)]
public sealed partial class ServerHousingBuildState
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public uint HousingPlotIndex { get; set; }
    [UInt(32)] public uint BuildStage { get; set; } // changes the model sequence for the prop, see tbl housingBuild.modelSequenceId
    [UInt(3)] public BuildState State { get; set; }
}
