using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Chooses the settings for the instance behind the open settings window: the request GameLib.SetInstanceSettings sends.
/// </summary>
/// <remarks>
/// The client clears its kept portal after sending it.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0163, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InstanceSettingsSet
{
    /// <summary>
    /// The portal, by unit id: the one <see cref="InstanceGameModeDialogShow"/> gave, which the client keeps while the settings window is open.
    /// </summary>
    [UInt(32)] public uint PortalUnitId { get; private set; }

    /// <summary>
    /// The difficulty chosen; the client sends nothing for a value of 2 or more.
    /// </summary>
    [UInt(2)] public WorldDifficulty Difficulty { get; private set; }

    /// <summary>
    /// The prime level chosen, 0 when none.
    /// </summary>
    [UInt(8)] public byte PrimeLevel { get; private set; }

    /// <summary>
    /// Bit 0 asks for level scaling, from the call's second argument. The client sends every other bit as 0.
    /// </summary>
    [UInt(11)] public ushort Flags { get; private set; }
}
