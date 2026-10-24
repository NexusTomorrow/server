using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Opens the instance restricted window: the client raises ShowInstanceRestrictedDialog.
/// </summary>
/// <remarks>
/// It leaves the client's kept portal as it was.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0158, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InstanceRestrictedDialogShow
{
    /// <summary>
    /// The client gives this value no meaning: its handler never reads it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
