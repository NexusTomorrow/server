using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventObjectiveNotificationMode)]
[WireMessage(GameMessageOpcode.ServerPublicEventObjectiveNotificationMode, Direction.ServerToClient)]
public sealed partial class ServerPublicEventObjectiveNotificationMode : IWritable
{
    [UInt(14)] public uint ObjectiveId { get; set; }
    [UInt(32)] public PublicEventObjectiveNotificationMode NotificationMode { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 14);
        writer.Write(NotificationMode, 32u);
    }
}
