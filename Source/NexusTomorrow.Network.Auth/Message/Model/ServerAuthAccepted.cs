using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model
{
    [Message(GameMessageOpcode.ServerAuthAccepted)]
    public class ServerAuthAccepted : IWritable
    {
        public uint DisconnectedForLag { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(DisconnectedForLag);
        }
    }
}
