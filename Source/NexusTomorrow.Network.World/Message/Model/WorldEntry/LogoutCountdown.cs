using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Starts the countdown before a logout completes, or cancels the pending request.
/// </summary>
[WireMessage(GameMessageOpcode.Server0092, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class LogoutCountdown
{
    [WireStruct]
    public sealed partial class LogoutSignatureBonuses
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [FixedArray(15), UInt(64)] public ulong[] Unknown2 { get; set; } = new ulong[15];
        [FixedArray(19), UInt(64)] public ulong[] Unknown3 { get; set; } = new ulong[19];
    }

    /// <summary>
    /// How long the countdown runs before the client confirms; with 0 the deadline is now.
    /// </summary>
    [UInt(32)] public uint Duration { get; set; }

    /// <summary>
    /// Read only when <see cref="Duration"/> is 0: true cancels, as does having no request pending.
    /// </summary>
    [UInt(1)] public bool Cancel { get; set; }

    public LogoutSignatureBonuses Unknown2 { get; set; } = new();
}
