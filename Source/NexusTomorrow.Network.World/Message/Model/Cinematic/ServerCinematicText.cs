using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicText, Direction.ServerToClient)]
public sealed partial class ServerCinematicText
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(21)] public uint LocalizedTextId { get; set; }
    [UInt(1)] public bool Unused { get; set; }
}
