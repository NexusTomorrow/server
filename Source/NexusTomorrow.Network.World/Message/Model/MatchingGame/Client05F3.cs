using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05F3, Direction.ClientToServer)]
public sealed partial class Client05F3
{
    public Client05EFPart0 Unknown0 { get; private set; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
