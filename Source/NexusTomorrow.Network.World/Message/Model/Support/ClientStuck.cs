using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

// Initiates a spell cast to unstick the player
[Message(GameMessageOpcode.ClientStuck)]
[WireMessage(GameMessageOpcode.ClientStuck, Direction.ClientToServer)]
public sealed partial class ClientStuck : IReadable
{
    [UInt(3)] public UnstickType UnstickingType { get; private set; }
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnstickingType = reader.ReadEnum<UnstickType>(3u);
        ClientSpellCastUniqueId = reader.ReadUInt();
    }
}
