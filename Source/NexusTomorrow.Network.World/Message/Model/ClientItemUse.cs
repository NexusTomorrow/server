using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientItemUse, Direction.ClientToServer)]
public sealed partial class ClientItemUse
{
    [UInt(32)] public uint CastingId { get; private set; }
    public ItemLocation Location { get; } = new();
    [UInt(32)] public uint TargetUnitId { get; private set; }
    public ItemLocation TargetLocation { get; } = new();
    public Position Position { get; } = new Position();
}
