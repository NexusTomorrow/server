using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// The client's trigger for its QuestPeriodicReset event.
/// </summary>
/// <remarks>
/// Only when <see cref="Unknown0"/> differs from the value the client holds does it act: it stores the value and raises
/// QuestPeriodicReset with <see cref="Unknown1"/>'s four bits.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0362, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class QuestPeriodicReset
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
