using System.Globalization;
using System.Numerics;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Commands;

/// <summary>
/// The character commands. Each posts to the realm owner and answers when the map holding the character does.
/// </summary>
public static class CharacterCommands
{
    /// <summary>
    /// The character commands, each posting to <paramref name="realm"/>.
    /// </summary>
    public static IEnumerable<CommandDefinition> Create(IRealmInbox realm)
    {
        yield return new CommandDefinition
        {
            Name = "character relocate",
            Description = "Move a character in the world to a position, and a facing in radians (0 when left out).",
            Permission = PermissionKeys.CharacterRelocate,
            Parameters =
            [
                new("characterId", "the character's id", IsValid: IsId, Invalid: "A character id is a whole number."),
                new("x", "the position's x", IsValid: IsFinite, Invalid: "A coordinate is a finite number."),
                new("y", "the position's y", IsValid: IsFinite, Invalid: "A coordinate is a finite number."),
                new("z", "the position's z", IsValid: IsFinite, Invalid: "A coordinate is a finite number."),
                new("facing", "the yaw in radians", Optional: true, IsValid: IsFinite, Invalid: "A facing is a finite number."),
            ],
            ExecuteAsync = async (invocation, cancellationToken) =>
            {
                IReadOnlyList<string> arguments = invocation.Arguments;
                ulong characterId = ulong.Parse(arguments[0], NumberStyles.None, CultureInfo.InvariantCulture);
                var position = new Vector3(Number(arguments[1]), Number(arguments[2]), Number(arguments[3]));
                float facing = arguments.Count > 4 ? Number(arguments[4]) : 0;
                var reply = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                if (!realm.Post(new RelocateRequested(characterId, position, facing, reply)))
                    return new CommandReply(CommandOutcome.Refused, "the server is stopping");

                return await reply.Task.WaitAsync(cancellationToken)
                    ? new CommandReply(CommandOutcome.Done, "relocated")
                    : new CommandReply(CommandOutcome.Refused, "the character is not in the world");
            },
        };
    }

    private static bool IsId(string value) => ulong.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out _);

    private static bool IsFinite(string value) =>
        float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float number) && float.IsFinite(number);

    private static float Number(string value) => float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
}
