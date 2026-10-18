namespace NexusTomorrow.Transport;

/// <summary>
/// The opcodes the client's network layer handles itself, before any message dispatch. They travel as bare frames, never inside an
/// envelope.
/// </summary>
public static class TransportOpcodes
{
    /// <summary>
    /// A ping: a one-byte index, 0 to 19. The receiver answers with <see cref="PingAnswer"/> carrying the same byte.
    /// </summary>
    public const ushort Ping = 0;

    /// <summary>
    /// The answer to a <see cref="Ping"/>, echoing its index byte.
    /// </summary>
    public const ushort PingAnswer = 1;

    /// <summary>
    /// An opcode the client's network layer accepts and ignores.
    /// </summary>
    public const ushort Ignored = 2;
}
