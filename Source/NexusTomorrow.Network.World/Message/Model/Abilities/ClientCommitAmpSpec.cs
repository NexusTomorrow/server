using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Submits the currently selected AMPs so they are saved to the player's current spec.
[Message(GameMessageOpcode.ClientCommitAmpSpec)]
[WireMessage(GameMessageOpcode.ClientCommitAmpSpec, Direction.ClientToServer)]
public sealed partial class ClientCommitAmpSpec : IReadable
{
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; } = [];

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt(7u);
        for (uint i = 0; i < count; i++)
        {
            Amps.Add(reader.ReadUShort());
        }
    }
}
