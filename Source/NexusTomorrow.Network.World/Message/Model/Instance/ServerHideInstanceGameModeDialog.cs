using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Fired when the server wants to close the instance window after a ShowInstanceGameModeDialog event.
[WireMessage(GameMessageOpcode.ServerHideInstanceGameModeDialog, Direction.ServerToClient)]
public sealed partial class ServerHideInstanceGameModeDialog
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
