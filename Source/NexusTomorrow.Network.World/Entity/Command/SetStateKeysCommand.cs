using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.State;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetStateKeys)]
[WireStruct]
[UnionCase(EntityCommand.SetStateKeys)]
public sealed partial class SetStateKeysCommand : IEntityCommandModel
{
    [UInt(8), Count] public byte TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<StateFlags> States { get; set; } = [];
    [UInt(32)] public uint Offset { get; set; }

    public void Read(GamePacketReader reader)
    {
        uint Count = reader.ReadByte();

        for (int i = 0; i < Count; i++)
            Times.Add(reader.ReadUInt());

        for (int i = 0; i < Count; i++)
            States.Add(reader.ReadEnum<StateFlags>(32));

        Offset = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Times.Count, 8u);

        foreach (uint time in Times)
            writer.Write(time);

        foreach (StateFlags state in States)
            writer.Write(state, 32);

        writer.Write(Offset);
    }
}
