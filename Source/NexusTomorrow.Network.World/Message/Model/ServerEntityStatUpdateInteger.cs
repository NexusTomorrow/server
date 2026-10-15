using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Sent to set an integer stat. The layout matches <see cref="ServerEntityStatUpdateFloat"/> except that the value is an integer,
/// which <see cref="IntegerStat"/> carries.
/// </summary>
[WireMessage(GameMessageOpcode.ServerEntityStatUpdateInteger, Direction.ServerToClient)]
public sealed partial class ServerEntityStatUpdateInteger
{
    [WireStruct]
    public sealed partial class IntegerStatValue
    {
        [UInt(5)] public Stat Stat { get; set; }
        [UInt(32)] public uint Value { get; set; }
    }

    [UInt(32)] public uint UnitId { get; set; }
    public IntegerStatValue IntegerStat { get; set; } = new();
}
