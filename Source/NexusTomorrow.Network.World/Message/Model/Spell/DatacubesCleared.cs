using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server00DF, Direction.ServerToClient)]
public sealed partial class DatacubesCleared
{
}
