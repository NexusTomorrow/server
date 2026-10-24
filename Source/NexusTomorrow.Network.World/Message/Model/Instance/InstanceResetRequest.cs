using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Asks to reset the instance behind the open settings window: the request GameLib.ResetSingleInstance sends.
/// </summary>
/// <remarks>
/// The client keeps its portal after sending it.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0153, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InstanceResetRequest
{
    /// <summary>
    /// The portal, by unit id: the one <see cref="InstanceGameModeDialogShow"/> gave, which the client keeps while the settings window is open.
    /// </summary>
    [UInt(32)] public uint PortalUnitId { get; private set; }
}
