using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[Message(GameMessageOpcode.ServerPathUnlockResult)]
[WireMessage(GameMessageOpcode.ServerPathUnlockResult, Direction.ServerToClient)]
public sealed partial class ServerPathUnlockResult : IWritable
{
    [UInt(8)] public GenericError Result { get; set; }
    [UInt(32)] public PathUnlockedMask UnlockedPathMask { get; set; }
    [Pad(32)] public Pad UnlockedPathMaskHigh { get; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 8u);
        writer.Write(UnlockedPathMask);
    }
}
