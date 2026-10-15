using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerSpellAbilityCharges)]
[WireMessage(GameMessageOpcode.ServerSpellAbilityCharges, Direction.ServerToClient)]
public sealed partial class ServerSpellAbilityCharges : IWritable
{
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint AbilityChargeCount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellId, 18u);
        writer.Write(AbilityChargeCount);
    }
}

