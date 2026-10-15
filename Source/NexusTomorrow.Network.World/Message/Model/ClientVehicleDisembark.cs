using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientVehicleDisembark)]
    public class ClientVehicleDisembark : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
