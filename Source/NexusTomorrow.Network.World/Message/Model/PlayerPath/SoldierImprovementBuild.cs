using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client067D, Direction.ClientToServer)]
public sealed partial class SoldierImprovementBuild
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
