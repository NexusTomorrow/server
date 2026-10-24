using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server068D, Direction.ServerToClient)]
public sealed partial class PetFlairUnlocked
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
