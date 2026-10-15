using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

// Sends all the saved costumes to the client, both personal and mannequin costumes.
// Max 12 personal costumes and 5 mannequin costumes. Indexes higher than that are ignored by the client.
[Message(GameMessageOpcode.ServerCostumeList)]
[WireMessage(GameMessageOpcode.ServerCostumeList, Direction.ServerToClient)]
public sealed partial class ServerCostumeList : IWritable
{
    [Array(CountBits = 32)] public List<Costume> Costumes { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Costumes.Count);
        Costumes.ForEach(c => c.Write(writer));
    }
}
