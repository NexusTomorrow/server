using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ServerProfessionsLoad, Direction.ServerToClient)]
public sealed partial class ServerProfessionsLoad
{
    [WireStruct]
    public sealed partial class DiscoveredSchematic
    {
        [UInt(32)] public uint TradeskillSchematic2Id { get; set; }
        [Float32] public Vector2 Coordinates { get; set; } // Coordinates on the discovery panel
    }

    [Array(CountBits = 32)] public List<TradeskillInfo> Tradeskills { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> LearnedSchematics { get; set; } = []; // TradeskillSchematic2Id
    [Array(CountBits = 32)] public List<DiscoveredSchematic> DiscoveredSchematics { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> LearnedSchematicDiscoveredFlags { get; set; } = []; // which LearnedSchematics were discovered, bit order matches LearnedSchematics array
    [UInt(32)] public uint RelearnCooldown { get; set; } // Sent as an offset from the time now, to the finish time, in milliseconds.
}
