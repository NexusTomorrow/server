using System.Text;
using NexusTomorrow.Gateway.Sts;
using NexusTomorrow.Network.Sts.Model;

namespace NexusTomorrow.Gateway.Tests.Sts;

public sealed class StsResponseWriterTests
{
    [Fact]
    public void AResponseHasTwoSpacesBeforeItsStatusTextAndCountsBodyBytes()
    {
        byte[] bytes = StsResponseWriter.Write(200, "OK", "4", "é\n");
        Assert.Equal("STS/1.0 200  OK\r\nl:3\r\ns:4R\r\n\r\né\n", Encoding.UTF8.GetString(bytes));
    }

    [Fact]
    public void XmlHasNoDeclarationAndOneElementPerLine()
    {
        string xml = StsResponseWriter.Xml(new RequestGameTokenResponse { Token = "t" });
        Assert.Equal("<Reply>\n<Token>t</Token>\n</Reply>", xml);
    }

    [Fact]
    public void ListMyAccountsWritesItsPropertiesNotFixedText()
    {
        string xml = StsResponseWriter.Xml(new ListMyAccountsResponse { Alias = "alice", Created = "then", GameAccountId = "7" });
        Assert.Equal(
            "<Reply type=\"array\">\n<GameAccount>\n<Alias>alice</Alias>\n<Created>then</Created>\n<GameAccountId>7</GameAccountId>\n</GameAccount>\n</Reply>",
            xml);
    }
}
