using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicStoryPanelDialogHide, Direction.ServerToClient)]
public sealed partial class ServerCinematicStoryPanelDialogHide
{
    [UInt(32)] public uint Delay { get; set; }
}
