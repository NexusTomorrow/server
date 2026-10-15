using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class VehiclePassenger
{
    [UInt(2)] public VehicleSeatType SeatType { get; set; }
    [UInt(3)] public byte SeatPosition { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
