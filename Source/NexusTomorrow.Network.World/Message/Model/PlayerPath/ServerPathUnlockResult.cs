using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[WireMessage(GameMessageOpcode.ServerPathUnlockResult, Direction.ServerToClient)]
public sealed partial class ServerPathUnlockResult
{
    [UInt(8)] public GenericError Result { get; set; }
    [UInt(32)] public PathUnlockedMask UnlockedPathMask { get; set; }
    [Pad(32)] public Pad UnlockedPathMaskHigh { get; }
}
