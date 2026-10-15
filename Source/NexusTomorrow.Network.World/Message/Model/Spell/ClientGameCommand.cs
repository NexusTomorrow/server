using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientGameCommand)]
[WireMessage(GameMessageOpcode.ClientGameCommand, Direction.ClientToServer)]
public sealed partial class ClientGameCommand : IReadable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(4)] public byte AbilityItemIndex { get; private set; }
    [UInt(4)] public byte LimitedActionSetId { get; private set; }
    [UInt(32)] public uint TargetUnitId { get; private set; }
    public Position TargetPosition { get; private set; } = new Position();

    public void Read(GamePacketReader reader)
    {
        ClientSpellCastUniqueId = reader.ReadUInt();
        AbilityItemIndex = reader.ReadByte(4u);
        LimitedActionSetId = reader.ReadByte(4u);
        TargetUnitId = reader.ReadUInt();
        TargetPosition.Read(reader);
    }
}
