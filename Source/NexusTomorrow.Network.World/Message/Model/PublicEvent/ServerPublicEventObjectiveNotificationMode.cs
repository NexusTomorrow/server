using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventObjectiveNotificationMode, Direction.ServerToClient)]
public sealed partial class ServerPublicEventObjectiveNotificationMode
{
    [UInt(15)] public uint ObjectiveId { get; set; }
    [UInt(32)] public PublicEventObjectiveNotificationMode NotificationMode { get; set; }
}
