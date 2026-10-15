using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventObjectiveNotificationMode)]
public class ServerPublicEventObjectiveNotificationMode : IWritable
{
    public uint ObjectiveId { get; set; }
    public PublicEventObjectiveNotificationMode NotificationMode { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ObjectiveId, 14);
        writer.Write(NotificationMode, 32u);
    }
}
