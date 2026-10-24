using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client050D, Direction.ClientToServer)]
public sealed partial class Client050D
{
    [FixedArray(6), UInt(32)] public uint[] Unknown0 { get; private set; } = new uint[6];
    [FixedArray(6)] public Client04F5Part0[] Unknown1 { get; private set; } = [.. Enumerable.Range(0, 6).Select(_ => new Client04F5Part0())];
}
