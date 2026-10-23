using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server08B8, Direction.ServerToClient)]
public sealed partial class UnitPathTypeChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
}
