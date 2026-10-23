using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Names the scientist's scanbot.
/// </summary>
/// <remarks>
/// PlayerPathLib.SetScannerName sends it.
/// </remarks>
[WireMessage(GameMessageOpcode.Client068C, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ScanbotRename
{
    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(2)] public byte Unknown0 { get; private set; }

    /// <summary>
    /// The scanbot profile, as <see cref="ScanbotSummon.Profile"/> carries it.
    /// </summary>
    [UInt(32)] public uint Profile { get; private set; }

    /// <summary>
    /// The name, cut to 16 characters.
    /// </summary>
    [WideString] public string Name { get; private set; } = "";
}
