using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientSummonVanityPet, Direction.ClientToServer)]
public sealed partial class ClientSummonVanityPet
{
    [UInt(18)] public uint Spell4BaseId { get; private set; }
}
