using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientSummonVanityPet)]
[WireMessage(GameMessageOpcode.ClientSummonVanityPet, Direction.ClientToServer)]
public sealed partial class ClientSummonVanityPet : IReadable
{
    [UInt(18)] public uint Spell4BaseId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Spell4BaseId = reader.ReadUInt(18);
    }
}
