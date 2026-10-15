using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientResurrectAccept, Direction.ClientToServer)]
public sealed partial class ClientResurrectAccept
{
    [UInt(32)] public uint UnitId { get; private set; }
    [UInt(32)] public ResurrectionType RezType { get; private set; }
}
