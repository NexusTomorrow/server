namespace NexusTomorrow.Network.Codec.Tests;

public sealed class NetworkGuidTests
{
    // The GUID whose wire bytes count 00 to 0F: Data1, Data2 and Data3 are little-endian, Data4 is in order.
    private static readonly Guid s_counting = new("03020100-0504-0706-0809-0a0b0c0d0e0f");

    [Fact]
    public void FromGuidSplitsTheFieldsAsTheWireCarriesThem()
    {
        var value = NetworkGuid.FromGuid(s_counting);
        Assert.Equal(0x03020100, value.Data1);
        Assert.Equal((short)0x0504, value.Data2);
        Assert.Equal((short)0x0706, value.Data3);
        Assert.Equal(Convert.FromHexString("08090A0B0C0D0E0F"), value.Data4);
    }

    [Fact]
    public void TheWireBytesAreTheGuidsBytesInOrder()
    {
        var value = NetworkGuid.FromGuid(s_counting);
        byte[] buffer = new byte[16];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        Assert.Equal(Convert.FromHexString("000102030405060708090A0B0C0D0E0F"), buffer);
    }

    [Fact]
    public void FromBytesAndCopyToRoundTrip()
    {
        byte[] bytes = Convert.FromHexString("000102030405060708090A0B0C0D0E0F");
        var value = NetworkGuid.FromBytes(bytes);
        Assert.Equal(s_counting, value.ToGuid());
        byte[] copy = new byte[16];
        value.CopyTo(copy);
        Assert.Equal(bytes, copy);
    }
}
