using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part9
{
    public interface IServer0262Union3 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0262Union3Arm00 : IServer0262Union3
    {
        [UInt(1)] public byte Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0262Union3Arm01 : IServer0262Union3
    {
        /// <summary>
        /// The damage or heal data. The client applies it only for a damage or heal effect.
        /// </summary>
        [VerifiedMember] public Server0262Part10 Unknown0 { get; set; } = new();
    }

    /// <summary>
    /// The effect, as an id in the client's Spell4Effects table. The row's type picks how the client applies it.
    /// </summary>
    [VerifiedMember, UInt(19)] public uint Spell4EffectId { get; set; }

    /// <summary>
    /// The effect's own id. The client finds the effect on its unit by it when a later message names the effect.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint EffectUniqueId { get; set; }

    /// <summary>
    /// The time before the effect applies, in milliseconds. With 0 the client applies it at once; a damage or heal effect's combat log
    /// entry waits for it, plus the effect's travel time.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Delay { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// Which data the effect carries: 1 carries the damage or heal data in <see cref="Server0262Union3Arm01"/>, and 0 carries
    /// <see cref="Server0262Union3Arm00"/>. It selects <see cref="Unknown5"/>.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Unknown4 { get; set; }

    /// <summary>
    /// The effect's data, in the form <see cref="Unknown4"/> selects.
    /// </summary>
    [VerifiedMember, Union(nameof(Unknown4))] public IServer0262Union3 Unknown5 { get; set; } = new Server0262Union3Arm00();
}
