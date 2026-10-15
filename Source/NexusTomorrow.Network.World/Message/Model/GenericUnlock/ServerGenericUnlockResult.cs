using NexusTomorrow.Game.Static.GenericUnlock;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[Message(GameMessageOpcode.ServerGenericUnlockResult)]
[WireMessage(GameMessageOpcode.ServerGenericUnlockResult, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockResult : IWritable
{
    [UInt(3)] public GenericUnlockResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 3u);
    }
}
