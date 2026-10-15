namespace NexusTomorrow.Network;

public class FragmentedBuffer
{
    public bool IsComplete => _data.Length == _offset;

    public byte[] Data
    {
        get
        {
            if (!IsComplete)
                throw new InvalidOperationException();
            return _data;
        }
    }

    private uint _offset;
    private readonly byte[] _data;

    public FragmentedBuffer(uint size)
    {
        _data = new byte[size];
    }

    public void Populate(GamePacketReader reader)
    {
        if (IsComplete)
            throw new InvalidOperationException();

        uint remaining = reader.BytesRemaining;
        if (remaining < _data.Length - _offset)
        {
            // not enough data, push entire frame into packet
            byte[] newData = reader.ReadBytes(remaining);
            Buffer.BlockCopy(newData, 0, _data, (int)_offset, (int)remaining);

            _offset += (uint)newData.Length;
        }
        else
        {
            // enough data, push required frame into packet
            byte[] newData = reader.ReadBytes((uint)_data.Length - _offset);
            Buffer.BlockCopy(newData, 0, _data, (int)_offset, newData.Length);

            _offset += (uint)newData.Length;
        }
    }
}
