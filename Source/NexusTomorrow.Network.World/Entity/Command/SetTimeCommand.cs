using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 0: sets the time of the commands after it.
/// </summary>
[WireStruct]
[UnionCase(0)]
public sealed partial class SetTimeCommand : IServer0262Union2
{
    /// <summary>
    /// The time of the commands after it, in milliseconds on the sender's clock. The client maps it to its own clock through its fit of
    /// the sender's clock for the unit, the fit <see cref="Message.Model.Movement.UnitMovement.Time"/> feeds.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Time { get; set; }
}
