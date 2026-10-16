using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client033F, Direction.ClientToServer)]
public sealed partial class Client033F
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [Union(nameof(Unknown0))] public IClient033FUnion0 Unknown1 { get; private set; }
    [WideString] public string Unknown2 { get; private set; }
}
