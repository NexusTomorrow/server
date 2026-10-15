using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientDash)]
[WireMessage(GameMessageOpcode.ClientDash, Direction.ClientToServer)]
public sealed partial class ClientDash : IReadable
{
    [UInt(3)] public DashDirection DashDirection { get; set; }

    public void Read(GamePacketReader reader)
    {
        DashDirection = reader.ReadEnum<DashDirection>(3);
    }
}
