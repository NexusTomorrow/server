using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    // Called from RequestCamp, ExitGame, Camp, CancelExit lua functions
    [Message(GameMessageOpcode.ClientLogoutRequest)]
    public class ClientLogoutRequest : IReadable
    {
        public bool Initiated { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Initiated = reader.ReadBit();
        }
    }
}
