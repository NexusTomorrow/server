using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerSchematicRemoveLearned)]
[WireMessage(GameMessageOpcode.ServerSchematicRemoveLearned, Direction.ServerToClient)]
public sealed partial class ServerSchematicRemoveLearned : IWritable
{
    [UInt(32)] public TradeskillType TradeskillId { get; set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeskillId, 32u);
        writer.Write(TradeskillSchematic2Id);
    }
}
