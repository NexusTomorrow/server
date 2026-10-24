using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Continues a cinematic camera held after its fade-out by <see cref="CameraAddFlags.AwaitContinue"/>, which then fades back in.
/// </summary>
/// <remarks>
/// After a <see cref="CinematicWhiteOut"/>, the client keeps the continue until <see cref="CinematicEnd"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server021F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicContinue
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(8)] public byte Unknown0 { get; set; }
}
