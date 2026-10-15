using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingRoleCheckStarted)]
[WireMessage(GameMessageOpcode.ServerMatchingRoleCheckStarted, Direction.ServerToClient)]
public sealed partial class ServerMatchingRoleCheckStarted : IWritable
{
    [UInt(1)] public bool RolesRequired { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RolesRequired);
    }
}
