using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ClientCinematicState, Direction.ClientToServer)]
public sealed partial class ClientCinematicState
{
    [UInt(8)] public CinematicState State { get; private set; }
    [UInt(1)] public bool NotifyOfCancelledCameraAction { get; private set; } // set by message 0x218
}
