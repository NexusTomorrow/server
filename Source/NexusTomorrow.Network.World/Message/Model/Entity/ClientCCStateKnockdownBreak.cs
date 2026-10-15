using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// Only sent when the player is knocked down and dashes to break out of it.
[Message(GameMessageOpcode.ClientCCStateKnockdownBreak)]
[WireMessage(GameMessageOpcode.ClientCCStateKnockdownBreak, NexusTomorrow.Network.Codec.Direction.ClientToServer)]
public sealed partial class ClientCCStateKnockdownBreak : IReadable
{
    [UInt(3)] public DashDirection Direction { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Direction = reader.ReadEnum<DashDirection>(3u);
    }
}
