using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Tells the server the instance settings window closed: the request GameLib.OnClosedInstanceSettings sends.
/// </summary>
/// <remarks>
/// The client sends it only when the call's argument is true, and clears its kept portal either way. The shipped add-ons pass
/// true when the player closes the window, and otherwise the flag HideInstanceGameModeDialog carries: false after
/// <see cref="InstanceGameModeDialogHide"/>, true after the client's own hides.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00D2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InstanceSettingsClosed
{
    /// <summary>
    /// The portal, by unit id: the one <see cref="InstanceGameModeDialogShow"/> gave, which the client keeps while the settings window is open.
    /// </summary>
    [UInt(32)] public uint PortalUnitId { get; private set; }
}
