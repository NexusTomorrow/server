using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client06C5, Direction.ClientToServer)]
public sealed partial class PlayerReport
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(3)] public byte Unknown1 { get; private set; }
    [UInt(4)] public byte Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; } = "";
    [UInt(64)] public ulong Unknown4 { get; private set; }
    [Float32] public float Unknown5 { get; private set; }
    [UInt(1)] public bool Unknown6 { get; private set; }
}
