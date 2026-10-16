using System.Numerics;
using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client04E6, Direction.ClientToServer)]
public sealed partial class Client04E6
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(3)] public byte Unknown3 { get; private set; }
    [Float32] public Vector3 Unknown4 { get; private set; }
    [Float32] public Quaternion Unknown5 { get; private set; }
}
