using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.ClientFortuneFlipCard, Direction.ClientToServer)]
public sealed partial class ClientFortuneFlipCard
{
    [UInt(32)] public uint SelectedCardIndex { get; private set; }
}
