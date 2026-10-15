using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Sent when client uses lua function GameLib::SetCurrentClassInnateAbilityIndex or /setstance command.
[WireMessage(GameMessageOpcode.ClientSetStance, Direction.ClientToServer)]
public sealed partial class ClientSetStance
{
    [UInt(8)] public byte InnateIndex { get; private set; }
}
