using NexusTomorrow.Game.Static.Pregame;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireStruct]
public partial class RealmInfo
{
    [WireStruct]
    public sealed partial class AccountRealmData
    {
        [UInt(14)] public ushort RealmId { get; set; }
        [UInt(32)] public uint CharacterCount { get; set; }
        [WideString] public string LastPlayedCharacter { get; set; }
        [UInt(64)] public ulong LastPlayedTime { get; set; }
    }

    [UInt(32)] public uint RealmId { get; set; }
    [WideString] public string RealmName { get; set; }
    [UInt(32)] public uint RealmNoteStringId { get; set; }
    [UInt(32)] public RealmFlag Flags { get; set; }
    [UInt(2)] public RealmType Type { get; set; }
    [UInt(3)] public RealmStatus Status { get; set; }
    [UInt(3)] public RealmPopulation Population { get; set; }
    [UInt(32)] public uint Unused1 { get; set; }
    [FixedArray(16), UInt(8)] public byte[] Unused2 { get; set; } = new byte[16];
    public AccountRealmData AccountRealmInfo { get; set; }
    [UInt(16)] public ushort Unused3 { get; set; }
    [UInt(16)] public ushort Unused4 { get; set; }
    [UInt(16)] public ushort Unused5 { get; set; }
    [UInt(16)] public ushort Unused6 { get; set; }
}
