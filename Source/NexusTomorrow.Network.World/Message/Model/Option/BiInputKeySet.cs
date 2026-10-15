using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// If sent without a CharacterId, applies to all characters on the account
[Message(GameMessageOpcode.BiInputKeySet)]
[WireMessage(GameMessageOpcode.BiInputKeySet, Direction.Both)]
public sealed partial class BiInputKeySet : IReadable, IWritable
{
    [Array(CountBits = 32)] public List<Binding> Bindings { get; set; } = [];
    [UInt(64)] public ulong CharacterId { get; set; }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt();
        for (uint i = 0u; i < count; i++)
        {
            var binding = new Binding();
            binding.Read(reader);
            Bindings.Add(binding);
        }

        CharacterId = reader.ReadULong();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Bindings.Count);
        Bindings.ForEach(b => b.Write(writer));
        writer.Write(CharacterId);
    }
}
