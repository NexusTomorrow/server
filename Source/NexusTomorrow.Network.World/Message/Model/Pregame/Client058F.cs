using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client058F, Direction.ClientToServer)]
public sealed partial class Client058F
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [FixedArray(16), UInt(8)] public byte[] Unknown1 { get; private set; } = new byte[16];
    [UInt(64)] public ulong Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; } = "";
    [UInt(32)] public uint Unknown4 { get; private set; }
}
