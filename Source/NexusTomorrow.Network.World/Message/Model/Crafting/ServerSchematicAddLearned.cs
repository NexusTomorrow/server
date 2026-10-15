using System.Numerics;
using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerSchematicAddLearned, Direction.ServerToClient)]
public sealed partial class ServerSchematicAddLearned
{
    [UInt(32)] public TradeskillType TradeskillId { get; set; }
    [UInt(32)] public uint TradeskillSchematic2Id { get; set; }
    [Float32] public Vector2 DiscoveryCoordinates { get; set; } // Coordinates on the discovery panel
}
