using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientCommunicatorAction, Direction.ClientToServer)]
public sealed partial class ClientCommunicatorAction
{
    [UInt(15)] public uint QuestId { get; private set; }
    [UInt(1)] public bool Engaged { get; private set; } // 1 = Player engaged with communicator, 0 = ignored
}
