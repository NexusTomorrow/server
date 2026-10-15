using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A stat and its value as <see cref="ServerEntityStatUpdateFloat"/> carries it. No selector is on the wire: the opcode picks the encoding,
/// and <see cref="ServerEntityStatUpdateInteger"/> carries its own integer layout.
/// </summary>
[WireStruct]
public sealed partial class StatValueUpdate
{
    [UInt(5)] public Stat Stat { get; set; }
    [Float32] public float Value { get; set; }
}
