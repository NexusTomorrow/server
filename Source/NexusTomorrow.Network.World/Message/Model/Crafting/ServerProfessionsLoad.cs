using System.Numerics;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerProfessionsLoad)]
public class ServerProfessionsLoad : IWritable
{
    public class DiscoveredSchematic : IWritable
    {
        public uint TradeskillSchematic2Id { get; set; }
        public Vector2 Coordinates { get; set; } // Coordinates on the discovery panel

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TradeskillSchematic2Id);
            writer.Write(Coordinates.X);
            writer.Write(Coordinates.Y);
        }
    }

    public List<TradeskillInfo> Tradeskills { get; set; } = [];
    public List<uint> LearnedSchematics { get; set; } = []; // TradeskillSchematic2Id
    public List<DiscoveredSchematic> DiscoveredSchematics { get; set; } = [];
    public List<uint> LearnedSchematicDiscoveredFlags { get; set; } = []; // which LearnedSchematics were discovered, bit order matches LearnedSchematics array
    public uint RelearnCooldown { get; set; } // Sent as an offset from the time now, to the finish time, in milliseconds.

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
