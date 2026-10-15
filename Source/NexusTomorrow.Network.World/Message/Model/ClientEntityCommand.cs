using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientEntityCommand, Direction.ClientToServer)]
public sealed partial class ClientEntityCommand
{
    [UInt(32)] public uint Time { get; set; }
    [Array(CountBits = 32)] public List<NetworkEntityCommand> Commands { get; } = [];
}
