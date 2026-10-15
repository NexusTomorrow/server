namespace NexusTomorrow.Network.Sts;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MessageAttribute : Attribute
{
    public string Uri { get; }

    public MessageAttribute(string uri)
    {
        Uri = uri;
    }
}
