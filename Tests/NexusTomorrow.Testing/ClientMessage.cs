using System.Reflection;

namespace NexusTomorrow.Testing;

// Client messages declare private setters, since the server only reads them; a test that sends one sets its members through them.
public static class ClientMessage
{
    // Set the named member and return the message, so a fixture reads as one expression.
    public static T With<T>(T message, string member, object value)
        where T : class
    {
        PropertyInfo property = typeof(T).GetProperty(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new ArgumentException($"{typeof(T).Name} has no member {member}.");
        property.SetValue(message, value);
        return message;
    }
}
