using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellThresholdUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdUpdate
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(8)] public byte CurrentThreshold { get; set; }
}
