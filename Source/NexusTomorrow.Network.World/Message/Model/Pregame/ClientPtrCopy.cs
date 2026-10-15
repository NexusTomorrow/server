using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame
{
    // Sent on lua function GameLib::InitiatePTRCharacterCopy or /ptrcopy.
    [Message(GameMessageOpcode.ClientPtrCopy)]
    public class ClientPtrCopy : IReadable
    {
        public ulong CharacterId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            // Zero byte message
        }
    }
}
