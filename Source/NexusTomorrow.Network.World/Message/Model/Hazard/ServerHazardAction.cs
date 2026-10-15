using NexusTomorrow.Game.Static.Hazard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[Message(GameMessageOpcode.ServerHazardAction)]
[WireMessage(GameMessageOpcode.ServerHazardAction, Direction.ServerToClient)]
public sealed partial class ServerHazardAction : IWritable
{
    [UInt(14)] public uint HazardId { get; set; }
    [UInt(3)] public HazardAction Action { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(HazardId, 14u);
        writer.Write(Action, 3u);
    }
}
