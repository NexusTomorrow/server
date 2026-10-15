using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerCharacterFlagsUpdated, Direction.ServerToClient)]
public sealed partial class ServerCharacterFlagsUpdated
{
    [UInt(32)] public CharacterFlag Flags { get; set; }
}
