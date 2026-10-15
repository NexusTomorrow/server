using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityStatUpdateFloat, Direction.ServerToClient)]
public sealed partial class ServerEntityStatUpdateFloat
{
    [UInt(32)] public uint UnitId { get; set; }
    public StatValueUpdate Stat { get; set; }
}
