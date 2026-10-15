using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerUnlockMount)]
[WireMessage(GameMessageOpcode.ServerUnlockMount, Direction.ServerToClient)]
public sealed partial class ServerUnlockMount : IWritable
{
    [UInt(18)] public uint Spell4Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4Id, 18u);
    }
}

