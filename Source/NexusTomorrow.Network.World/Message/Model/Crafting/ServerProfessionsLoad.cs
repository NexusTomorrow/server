using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerProfessionsLoad)]
[WireMessage(GameMessageOpcode.ServerProfessionsLoad, Direction.ServerToClient)]
public sealed partial class ServerProfessionsLoad : IWritable
{
    [WireStruct]
    public sealed partial class DiscoveredSchematic : IWritable
    {
        [UInt(32)] public uint TradeskillSchematic2Id { get; set; }
        [Float32] public Vector2 Coordinates { get; set; } // Coordinates on the discovery panel

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TradeskillSchematic2Id);
            writer.Write(Coordinates.X);
            writer.Write(Coordinates.Y);
        }
    }

    [Array(CountBits = 32)] public List<TradeskillInfo> Tradeskills { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> LearnedSchematics { get; set; } = []; // TradeskillSchematic2Id
    [Array(CountBits = 32)] public List<DiscoveredSchematic> DiscoveredSchematics { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> LearnedSchematicDiscoveredFlags { get; set; } = []; // which LearnedSchematics were discovered, bit order matches LearnedSchematics array
    [UInt(32)] public uint RelearnCooldown { get; set; } // Sent as an offset from the time now, to the finish time, in milliseconds.

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Tradeskills.Count);
        foreach (TradeskillInfo tradeskill in Tradeskills)
        {
            tradeskill.Write(writer);
        }

        writer.Write(LearnedSchematics.Count);
        foreach (uint schematic in LearnedSchematics)
        {
            writer.Write(schematic);
        }

        writer.Write(DiscoveredSchematics.Count);
        foreach (DiscoveredSchematic schematic in DiscoveredSchematics)
        {
            schematic.Write(writer);
        }

        writer.Write(LearnedSchematicDiscoveredFlags.Count);
        foreach (uint id in LearnedSchematicDiscoveredFlags)
        {
            writer.Write(id);
        }

        writer.Write(RelearnCooldown);
    }
}
