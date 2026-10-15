using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ClientCinematicState)]
[WireMessage(GameMessageOpcode.ClientCinematicState, Direction.ClientToServer)]
public sealed partial class ClientCinematicState : IReadable
{
    [UInt(8)] public CinematicState State { get; private set; }
    [UInt(1)] public bool NotifyOfCancelledCameraAction { get; private set; } // set by message 0x218

    public void Read(GamePacketReader reader)
    {
        State = reader.ReadEnum<CinematicState>(8u);
        NotifyOfCancelledCameraAction = reader.ReadBit();
    }
}
