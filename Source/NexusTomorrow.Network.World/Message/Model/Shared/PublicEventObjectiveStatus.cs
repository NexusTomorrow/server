using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventObjectiveStatus
{
    [WireStruct]
    public sealed partial class VirtualItem
    {
        [UInt(14)] public uint ItemId { get; set; }
        [UInt(32)] public uint Count { get; set; }
    }

    /// <summary>
    /// The objective data <see cref="DataType"/> selects.
    /// </summary>
    public interface IPayload : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(PublicEventObjectiveDataType.Empty)]
    public sealed partial class EmptyData : IPayload
    {
        [Pad(8)] public Pad Unknown { get; }
    }

    [WireStruct]
    [UnionCase(PublicEventObjectiveDataType.CapturePointDefend)]
    public sealed partial class CapturePointDefendData : IPayload
    {
        [UInt(32)] public PublicEventTeam ControllingTeam { get; set; }
    }

    [WireStruct]
    [UnionCase(PublicEventObjectiveDataType.CapturePoint)]
    public sealed partial class CapturePointData : IPayload
    {
        [UInt(32)] public PublicEventTeam CapturingTeam { get; set; }
    }

    [WireStruct]
    [UnionCase(PublicEventObjectiveDataType.VirtualItemDepot)]
    public sealed partial class VirtualItemDepotData : IPayload
    {
        [Array(CountBits = 32)] public List<VirtualItem> VirtualItems { get; set; } = [];
    }

    [UInt(32)] public PublicEventStatus Status { get; set; }
    [UInt(32)] public uint ObjectiveData { get; set; } // Contents depends on objective type
                                                       // For ActivateTargetGroupChecklist and TalkToChecklist, is a bitfield for objective completion
                                                       // For ContestedArea and CapturePoint, is PublicEventTeamId indicating team owning the objective
    [UInt(32)] public uint DynamicMax { get; set; }
    [Float32] public float Count { get; set; }
    [UInt(32)] public uint UnkState { get; set; }
    [UInt(3)] public PublicEventObjectiveDataType DataType { get; set; }
    [Union(nameof(DataType))] public IPayload Payload { get; set; } = new EmptyData();
}
