using System.Reflection;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

public sealed class DefaultWritabilityTests
{
    private static readonly Assembly[] s_models =
    [
        typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
    ];

    // A default instance is what the registry creates for a client message, so a null list, string or layout member, or a fixed array
    // holding nulls, is a message the codec cannot write.
    [Fact]
    public void EveryDeclaredMessagesDefaultIsWritable()
    {
        Type[] messages =
        [
            .. s_models.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttribute<WireMessageAttribute>() is not null)
                .OrderBy(type => type.FullName, StringComparer.Ordinal),
        ];
        var problems = new List<string>();
        foreach (Type type in messages)
        {
            try
            {
                var message = (IWireStruct)Activator.CreateInstance(type, nonPublic: true)!;
                string? held = FirstNullMember(message, type.Name);
                if (held is not null)
                {
                    problems.Add($"{type.Name}: {held} is null");
                    continue;
                }

                byte[] written = Write(message);
                var read = (IWireStruct)Activator.CreateInstance(type, nonPublic: true)!;
                var reader = new BitReader(written);
                read.Read(ref reader);
                if (!Write(read).AsSpan().SequenceEqual(written))
                    problems.Add($"{type.Name}: reads back to other bytes");
            }
            catch (Exception exception)
            {
                problems.Add($"{type.Name}: {exception.GetType().Name}: {exception.Message}");
            }
        }

        TestContext.Current.TestOutputHelper?.WriteLine($"{messages.Length} declared messages written and read back.");
        Assert.True(messages.Length > 200, $"only {messages.Length} messages were found");
        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
    }

    private static byte[] Write(IWireStruct value)
    {
        byte[] buffer = new byte[(value.MeasureBits() + 7) / 8];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        return buffer;
    }

    // Any accessibility, as the generator takes it; a [Pad] member and one marked [NotOnWire] are not written from a held value.
    private static string? FirstNullMember(object value, string path)
    {
        foreach (MemberInfo member in value.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (member is not (PropertyInfo { GetMethod: not null } or FieldInfo)
                || member is PropertyInfo indexer && indexer.GetIndexParameters().Length > 0
                || member.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false)
                || member.IsDefined(typeof(NotOnWireAttribute), inherit: false)
                || member.IsDefined(typeof(PadAttribute), inherit: false))
            {
                continue;
            }

            object? held = member is PropertyInfo property ? property.GetValue(value) : ((FieldInfo)member).GetValue(value);
            string where = path + "." + member.Name;
            if (held is null)
                return where;
            if (held is System.Collections.IEnumerable items and not string and not byte[])
            {
                int index = 0;
                foreach (object? item in items)
                {
                    if (item is null)
                        return $"{where}[{index}]";
                    if (item is IWireStruct && FirstNullMember(item, $"{where}[{index}]") is { } inner)
                        return inner;
                    index++;
                }
            }
            else if (held is IWireStruct && FirstNullMember(held, where) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }
}
