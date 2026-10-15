using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance
{
    // Fires OnInstanceResetResult event with false condition.
    // Not seen in sniffs probably because ServerInstanceResetResult can signal the same condition
    [Message(GameMessageOpcode.ServerInstanceResetFailed)]
    public class ServerInstanceResetFailed : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            // Zero byte message
        }
    }
}
