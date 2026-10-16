using System.Numerics;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client06D1, Direction.ClientToServer)]
public sealed partial class Client06D1
{
    [UInt(16)] public ushort Unknown0 { get; private set; }
    [UInt(16)] public ushort Unknown1 { get; private set; }
    [Float32] public Vector3 Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; }
    [WideString] public string Unknown4 { get; private set; }
    [UInt(32)] public uint Unknown5 { get; private set; }
}
