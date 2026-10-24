using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Tutorial;

[WireMessage(GameMessageOpcode.Server0179, Direction.ServerToClient)]
public sealed partial class TutorialShow
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
