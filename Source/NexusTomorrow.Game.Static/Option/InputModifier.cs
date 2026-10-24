namespace NexusTomorrow.Game.Static.Option;

/// <summary>
/// The modifier keys held with an input, as GameLib.CodeEnumInputModifier names them: a mask, so an input may take several.
/// </summary>
[Flags]
public enum InputModifier
{
    Shift = 1,
    Control = 2,
    Alt = 4,
}
