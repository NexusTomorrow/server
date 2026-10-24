using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server077F, Direction.ServerToClient)]
public sealed partial class PetDespawned
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
