using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientItemUse)]
[WireMessage(GameMessageOpcode.ClientItemUse, Direction.ClientToServer)]
public sealed partial class ClientItemUse : IReadable
{
    [UInt(32)] public uint CastingId { get; private set; }
    public ItemLocation Location { get; } = new();
    [UInt(32)] public uint TargetUnitId { get; private set; }
    public ItemLocation TargetLocation { get; } = new();
    public Position Position { get; } = new Position();

    public void Read(GamePacketReader reader)
    {
        CastingId = reader.ReadUInt();
        Location.Read(reader);
        TargetUnitId = reader.ReadUInt();
        TargetLocation.Read(reader);
        Position.Read(reader);
    }
}
