using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerNotifyUse)]
[WireMessage(GameMessageOpcode.ServerPathSettlerNotifyUse, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerNotifyUse : IWritable
{
    [UInt(14)] public ushort PathSettlerImprovementGroupId { get; set; }
    [WideString] public string Name { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerImprovementGroupId, 14);
        writer.WriteStringWide(Name);
    }
}
