using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerCharacterDisabled, Direction.ServerToClient)]
public sealed partial class ServerCharacterDisabled
{
    [UInt(64)] public ulong CharacterId { get; set; }
    [UInt(1)] public bool Disabled { get; set; }
}
