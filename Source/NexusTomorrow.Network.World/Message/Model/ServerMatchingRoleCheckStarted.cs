using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingRoleCheckStarted)]
public class ServerMatchingRoleCheckStarted : IWritable
{
    public bool RolesRequired { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RolesRequired);
    }
}
