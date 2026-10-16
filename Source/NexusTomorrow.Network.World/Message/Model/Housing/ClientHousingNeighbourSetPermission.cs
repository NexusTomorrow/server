using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0518, Direction.ClientToServer)]
public sealed partial class Client0518
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
