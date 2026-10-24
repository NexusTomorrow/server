using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// One input action's binding: its first three inputs, each a device, a code and the modifier keys held with it. The words are grouped
/// by field across the three inputs: the three devices, then the three codes, then the three modifiers.
/// </summary>
[WireStruct]
public sealed partial class InputKeyBinding
{
    /// <summary>
    /// The input action, by id in the client's InputAction table. The client looks the action's binding up by it.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort InputActionId { get; set; }

    /// <summary>
    /// The device of the action's first input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputDevice Device1 { get; set; }

    /// <summary>
    /// The device of the action's second input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputDevice Device2 { get; set; }

    /// <summary>
    /// The device of the action's third input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputDevice Device3 { get; set; }

    /// <summary>
    /// The code of the action's first input, by <see cref="Device1"/>: a keyboard scan code, or a mouse button as <see cref="InputMouse"/>
    /// numbers it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Code1 { get; set; }

    /// <summary>
    /// The code of the action's second input, by <see cref="Device2"/>, as <see cref="Code1"/> is by <see cref="Device1"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Code2 { get; set; }

    /// <summary>
    /// The code of the action's third input, by <see cref="Device3"/>, as <see cref="Code1"/> is by <see cref="Device1"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Code3 { get; set; }

    /// <summary>
    /// The modifier keys held with the action's first input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputModifier Modifier1 { get; set; }

    /// <summary>
    /// The modifier keys held with the action's second input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputModifier Modifier2 { get; set; }

    /// <summary>
    /// The modifier keys held with the action's third input.
    /// </summary>
    [VerifiedMember, UInt(32)] public InputModifier Modifier3 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0, and keeps the value it takes without reading it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown10 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0, and keeps the value it takes without reading it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown11 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0, and keeps the value it takes without reading it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown12 { get; set; }
}
