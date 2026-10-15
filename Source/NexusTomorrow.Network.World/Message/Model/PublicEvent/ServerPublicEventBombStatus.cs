using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventBombStatus)]
[WireMessage(GameMessageOpcode.ServerPublicEventBombStatus, Direction.ServerToClient)]
public sealed partial class ServerPublicEventBombStatus : IWritable
{
    [NotOnWire] public PublicEventTeam Team { get; set; }
    [UInt(32)] public uint CarrierUnitId { get; set; }
    [UInt(32)] public uint TimeInMs { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CarrierUnitId);
        writer.Write(TimeInMs);
    }
}
