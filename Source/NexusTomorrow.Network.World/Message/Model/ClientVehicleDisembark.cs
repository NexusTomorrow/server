using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientVehicleDisembark, Direction.ClientToServer)]
public sealed partial class ClientVehicleDisembark
{
}
