using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCancelEffect)]
[WireMessage(GameMessageOpcode.ClientCancelEffect, Direction.ClientToServer)]
public sealed partial class ClientCancelEffect : IReadable
{
    [UInt(32)] public uint ServerUniqueId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ServerUniqueId = reader.ReadUInt();
    }
}
