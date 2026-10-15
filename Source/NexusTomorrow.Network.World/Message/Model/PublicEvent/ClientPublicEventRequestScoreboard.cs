using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Toggles whether the PublicEventLiveStatsUpdate event will fire for this public event.
[Message(GameMessageOpcode.ClientPublicEventRequestScoreboard)]
[WireMessage(GameMessageOpcode.ClientPublicEventRequestScoreboard, Direction.ClientToServer)]
public sealed partial class ClientPublicEventRequestScoreboard : IReadable
{
    [UInt(14)] public uint EventId { get; private set; }
    [UInt(1)] public bool Subscribe { get; private set; }

    public void Read(GamePacketReader reader)
    {
        EventId = reader.ReadUInt(14u);
        Subscribe = reader.ReadBit();
    }
}
