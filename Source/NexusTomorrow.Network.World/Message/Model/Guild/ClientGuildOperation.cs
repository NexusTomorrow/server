using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientGuildOperation)]
[WireMessage(GameMessageOpcode.ClientGuildOperation, Direction.ClientToServer)]
public partial class ClientGuildOperation : IReadable
{
    /// <summary>
    /// The operation's 64-bit argument; each narrower view reads its low bytes.
    /// </summary>
    [WireStruct]
    public sealed partial class OperationData
    {
        [UInt(64)] public ulong UInt64Data { get; set; }
        [NotOnWire] public bool BoolData => (byte)UInt64Data != 0;
        [NotOnWire] public int Int32Data => unchecked((int)UInt64Data);
        [NotOnWire] public uint UInt32Data => unchecked((uint)UInt64Data);
    }

    public Identity GuildIdentity { get; private set; } = new();
    [UInt(32)] public uint Rank { get; private set; }
    public OperationData Data { get; private set; } = new();
    [WideString] public string TextValue { get; private set; } // Client has this named as "Who", but it is used for Player Names, Rank Names, Notes, MOTD, and other stuff
    [UInt(6)] public GuildOperation Operation { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        Rank = reader.ReadUInt();
        Data = new OperationData
        {
            UInt64Data = reader.ReadULong()
        };
        TextValue = reader.ReadWideString();
        Operation = reader.ReadEnum<GuildOperation>(6u);
    }
}
