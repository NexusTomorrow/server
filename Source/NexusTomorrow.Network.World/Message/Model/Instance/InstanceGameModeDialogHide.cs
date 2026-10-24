using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Closes the instance settings window.
/// </summary>
/// <remarks>
/// The client raises HideInstanceGameModeDialog with false, so the add-ons close the window and send nothing back. The
/// client's own hides pass true, and then <see cref="InstanceSettingsClosed"/> follows: among them, when the portal
/// interaction ends or another replaces it, and when the player answers a group invite.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00D3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InstanceGameModeDialogHide
{
    /// <summary>
    /// The client gives this value no meaning. Its handler never reads it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
