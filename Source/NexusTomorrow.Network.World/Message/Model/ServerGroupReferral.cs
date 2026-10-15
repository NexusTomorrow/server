using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupReferral)]
[WireMessage(GameMessageOpcode.ServerGroupReferral, Direction.ServerToClient)]
public sealed partial class ServerGroupReferral : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }

    public Identity InviteeIdentity { get; set; }

    [WideString] public string InviteeName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        InviteeIdentity.Write(writer);
        writer.WriteStringWide(InviteeName);
    }
}
