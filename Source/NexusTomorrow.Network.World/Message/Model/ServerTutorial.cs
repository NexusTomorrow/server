using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerTutorial, Direction.ServerToClient)]
public sealed partial class ServerTutorial
{
    [UInt(32)] public uint TutorialId { get; set; }
}
