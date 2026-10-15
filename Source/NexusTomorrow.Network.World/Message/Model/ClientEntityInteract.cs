using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEntityInteract)]
[WireMessage(GameMessageOpcode.ClientEntityInteract, Direction.ClientToServer)]
public sealed partial class ClientEntityInteract : IReadable
{
    [UInt(32)] public uint UnitId { get; private set; } // Interaction unit
    [UInt(7)] public InteractionType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
        Type = reader.ReadEnum<InteractionType>(7);
    }
}
