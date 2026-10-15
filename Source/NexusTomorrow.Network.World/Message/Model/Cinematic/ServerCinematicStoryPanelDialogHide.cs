using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicStoryPanelDialogHide)]
[WireMessage(GameMessageOpcode.ServerCinematicStoryPanelDialogHide, Direction.ServerToClient)]
public sealed partial class ServerCinematicStoryPanelDialogHide : IWritable
{
    [UInt(32)] public uint Delay { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
    }
}
