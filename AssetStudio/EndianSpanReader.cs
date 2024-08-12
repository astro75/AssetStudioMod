using System;
using System.Buffers.Binary;

namespace AssetStudio
{
    public static class EndianSpanReader
    {
        public static uint SpanToUint32(Span<byte> data, int start, bool isBigEndian)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadUInt32BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(start));
        }

        public static uint SpanToUint16(Span<byte> data, int start, bool isBigEndian)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadUInt16BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(start));
        }

        public static long SpanToInt64(Span<byte> data, int start, bool isBigEndian)
        {
            return isBigEndian
                ? BinaryPrimitives.ReadInt64BigEndian(data.Slice(start))
                : BinaryPrimitives.ReadInt64LittleEndian(data.Slice(start));
        }
    }
}
