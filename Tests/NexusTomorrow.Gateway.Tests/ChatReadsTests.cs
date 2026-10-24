using System.Reflection;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.World;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Chat;

namespace NexusTomorrow.Gateway.Tests;

public sealed class ChatReadsTests
{
    [Fact]
    public void EveryMemberChatReadsIsMarked()
    {
        Assert.NotEmpty(ChatReads.Members);
        foreach ((Type type, string member) in ChatReads.Members)
        {
            PropertyInfo? property = type.GetProperty(member);
            Assert.True(property is not null, $"{type.Name}.{member} does not exist");
            Assert.True(property.IsDefined(typeof(VerifiedMemberAttribute), inherit: false), $"{type.Name}.{member} is not marked verified");
        }
    }

    [Fact]
    public void TheSaysRowAdmitsItAtPurposeUnderTheRealmTagAndLimitsItsBody()
    {
        OpcodeRow row = OpcodeTable.All[0x01C3];

        Assert.Equal(MessageOwner.Realm, row.Owner);
        Assert.True(row.ReadsVerifiedMembers);
        Assert.Equal((byte)0x0B, row.Tag);
        Assert.Equal(4096, row.MaxBodyBytes);
        Assert.Equal(Rung.Purpose, MessageCatalog.Default.RungOf(typeof(ChatSend)));
    }
}
