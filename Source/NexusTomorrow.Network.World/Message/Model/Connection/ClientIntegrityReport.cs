using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Connection;

/// <summary>
/// The client's report on its own integrity: a check of the player unit's properties and of the rate of the client's clock, sent on a
/// timer of the client's own. Nothing on the client reads it back, and it needs no reply.
/// </summary>
[WireMessage(GameMessageOpcode.Client023C, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ClientIntegrityReport
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [Float32] public float Unknown3 { get; private set; }
    [Float32] public float Unknown4 { get; private set; }
    [Float32] public float Unknown5 { get; private set; }
    [UInt(32)] public uint Unknown6 { get; private set; }
}
