using System.Reflection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// A declared message as the reference pages list it.
/// </summary>
public sealed record ReferenceMessage(Type Type, ushort Opcode, Direction Direction, Rung? Rung, string Area)
{
    /// <summary>
    /// The message <paramref name="type"/> declares.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="type"/> carries no <see cref="WireMessageAttribute"/>.</exception>
    public static ReferenceMessage Of(Type type)
    {
        WireMessageAttribute message = type.GetCustomAttribute<WireMessageAttribute>()
            ?? throw new ArgumentException($"{type.Name} is not a declared message.", nameof(type));
        return new ReferenceMessage(type, (ushort)message.Opcode, message.Direction, type.GetCustomAttribute<VerifiedAttribute>()?.Rung, Areas.AreaOf(type));
    }
}

/// <summary>
/// The feature area a message belongs to: the namespace segment after <c>.Message.Model.</c>. A message at the root of the Network or
/// Network.Auth models is pregame; every World message sits in its area's folder.
/// </summary>
public static class Areas
{
    private const string ModelSegment = ".Message.Model";

    /// <summary>
    /// The area of <paramref name="message"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException"><paramref name="message"/> is outside every model namespace, or a World message at the
    /// model root.</exception>
    public static string AreaOf(Type message)
    {
        string space = message.Namespace ?? "";
        int at = space.IndexOf(ModelSegment, StringComparison.Ordinal);
        if (at < 0)
            throw new InvalidOperationException($"{message.FullName} is outside every model namespace.");

        string rest = space[(at + ModelSegment.Length)..];
        if (rest.Length == 0)
        {
            return space.StartsWith("NexusTomorrow.Network.World", StringComparison.Ordinal)
                ? throw new InvalidOperationException($"{message.FullName} sits at the World model root, in no area.")
                : "Pregame";
        }

        string area = rest.TrimStart('.');
        int dot = area.IndexOf('.', StringComparison.Ordinal);
        return dot >= 0 ? area[..dot] : area;
    }
}
