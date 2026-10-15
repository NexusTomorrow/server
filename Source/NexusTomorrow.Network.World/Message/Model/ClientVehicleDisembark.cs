using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientVehicleDisembark)]
[WireMessage(GameMessageOpcode.ClientVehicleDisembark, Direction.ClientToServer)]
public sealed partial class ClientVehicleDisembark : IReadable
{
    public void Read(GamePacketReader reader)
    {
    }
}
