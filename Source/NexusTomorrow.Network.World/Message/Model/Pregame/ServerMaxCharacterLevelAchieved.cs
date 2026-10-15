using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerMaxCharacterLevelAchieved, Direction.ServerToClient)]
public sealed partial class ServerMaxCharacterLevelAchieved
{
    [UInt(32)] public uint Level { get; set; }
}
