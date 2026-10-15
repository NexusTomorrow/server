using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.State;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetState)]
[WireStruct]
[UnionCase(EntityCommand.SetState)]
public sealed partial class SetStateCommand : IEntityCommandModel
{
    [UInt(32)] public StateFlags State { get; set; }

    public void Read(GamePacketReader reader)
    {
        State = reader.ReadEnum<StateFlags>(32);
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(State, 32);
    }
}
