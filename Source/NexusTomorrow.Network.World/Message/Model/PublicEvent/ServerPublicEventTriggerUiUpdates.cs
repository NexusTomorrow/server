using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Triggers a repeating event for BonusEventsChanged
[Message(GameMessageOpcode.ServerPublicEventTriggerUiUpdates)]
[WireMessage(GameMessageOpcode.ServerPublicEventTriggerUiUpdates, Direction.ServerToClient)]
public sealed partial class ServerPublicEventTriggerUiUpdates : IWritable
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(1)] public bool Start { get; set; } // true = triggers PublicEventStart, false = triggers PublicEventCleared

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14);
        writer.Write(Start);
    }
}
