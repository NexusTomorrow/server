using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireStruct]
public sealed partial class PlayerInfoBase : IWritable
{
    [UInt(3)] public byte ResultCode { get; set; } = 0; // values above zero indicate a failure. 5 = character deleted
    public Identity Identity { get; set; }
    [TerminatedWideString] public string Name { get; set; }
    [UInt(14)] public Faction Faction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ResultCode, 3u);
        Identity.Write(writer);
        writer.WriteStringFixed(Name);
        writer.Write(Faction, 14u);
    }
}
