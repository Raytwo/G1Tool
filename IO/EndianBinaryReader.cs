using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace G1Tool.IO
{
    public class EndianBinaryReader : BinaryReader
    {
        private StringBuilder mStringBuilder;
        private Endianness mEndianness;
        private Dictionary<long, object> mObjectLookup;

        public Endianness Endianness
        {
            get => mEndianness;
            set
            {
                SwapBytes = value != EndiannessHelper.SystemEndianness;
                mEndianness = value;
            }
        }

        public string FileName { get; set; }

        public bool SwapBytes { get; private set; }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public long Length => BaseStream.Length;

        public long BaseOffset { get; set; }

        public EndianBinaryReader(Stream input, Endianness endianness)
            : base(input)
        {
            FileName = input is FileStream fs ? fs.Name : null;
            Init(Encoding.Default, endianness);
        }

        public EndianBinaryReader( string filepath, Endianness endianness )
            : base(File.OpenRead(filepath))
        {
            FileName = filepath;
            Init( Encoding.Default, endianness );
        }

        public EndianBinaryReader(Stream input, Encoding encoding, Endianness endianness)
            : base(input, encoding)
        {
            FileName = input is FileStream fs ? fs.Name : null;
            Init(encoding, endianness);
        }

        public EndianBinaryReader( Stream input, bool leaveOpen, Endianness endianness )
            : base( input, Encoding.Default, leaveOpen )
        {
            FileName = input is FileStream fs ? fs.Name : null;
            Init( Encoding.Default, endianness );
        }

        public EndianBinaryReader(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness)
            : base(input, encoding, leaveOpen)
        {
            FileName = input is FileStream fs ? fs.Name : null;
            Init(encoding, endianness);
        }

        private void Init(Encoding encoding, Endianness endianness)
        {
            mStringBuilder = new StringBuilder();
            Endianness = endianness;
            BaseOffset = 0;
            mObjectLookup = new Dictionary<long, object> { [ 0 ] = null };
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
        }

        public void SeekBegin(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public void SeekCurrent(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.Current);
        }

        public void SeekEnd(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.End);
        }

        public bool IsValidOffset( int offset )
        {
            if ( offset == 0 )
                return true;

            if ( ( offset % 4 ) != 0 )
                return false;

            var effectiveOffset = offset + BaseOffset;
            return offset >= 0 && effectiveOffset >= 0 && effectiveOffset <= Length;
        }

        public void ReadOffset( Action action )
        {
            var offset = ReadInt32();
            if ( offset != 0 )
            {
                long current = Position;
                SeekBegin( offset + BaseOffset );
                action();
                SeekBegin( current );
            }
        }

        public void ReadOffset( Action<EndianBinaryReader> action )
        {
            var offset = ReadInt32();
            if ( offset != 0 )
            {
                long current = Position;
                SeekBegin( offset + BaseOffset );
                action( this );
                SeekBegin( current );
            }
        }

        public void ReadOffset( int count, Action<int> action )
        {
            ReadOffset( () =>
            {
                for ( var i = 0; i < count; ++i )
                    action( i );
            } );
        }

        public void ReadAtOffset( long offset, Action action )
        {
            if ( offset == 0 )
                return;

            long current = Position;
            SeekBegin( offset + BaseOffset );
            action();
            SeekBegin( current );
        }

        public void ReadAtOffset( long offset, Action<EndianBinaryReader> action )
        {
            if ( offset == 0 )
                return;

            long current = Position;
            SeekBegin( offset + BaseOffset );
            action( this );
            SeekBegin( current );
        }

        public void ReadAtOffset( long offset, int count, Action<int> action )
        {
            if ( offset == 0 )
                return;

            ReadAtOffset( offset, () =>
            {
                for ( var i = 0; i < count; ++i )
                    action( i );
            } );
        }

        internal void ReadAtOffset<T>( long offset, int count, List<T> list, object context = null ) where T : ISerializableObject, new()
        {
            if ( offset == 0 )
                return;

            ReadAtOffset( offset, () =>
            {
                for ( var i = 0; i < count; ++i )
                {
                    var item = new T();
                    item.Read( this, context );
                    list.Add( item );
                }
            } );
        }

        public sbyte[] ReadSBytes(int count)
        {
            var array = new sbyte[count];
            for (var i = 0; i < array.Length; i++)
                array[i] = ReadSByte();

            return array;
        }

        public bool[] ReadBooleans(int count)
        {
            var array = new bool[count];
            for (var i = 0; i < array.Length; i++)
                array[i] = ReadBoolean();

            return array;
        }

        public override short ReadInt16()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadInt16()) : base.ReadInt16();
        }

        public short[] ReadInt16s(int count)
        {
            var array = new short[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadInt16();
            }

            return array;
        }

        public override ushort ReadUInt16()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadUInt16()) : base.ReadUInt16();
        }

        public ushort[] ReadUInt16s(int count)
        {
            var array = new ushort[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadUInt16();
            }

            return array;
        }


        public decimal[] ReadDecimals(int count)
        {
            var array = new decimal[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadDecimal();
            }

            return array;
        }

        public double[] ReadDoubles(int count)
        {
            var array = new double[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadDouble();
            }

            return array;
        }

        public override int ReadInt32()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadInt32()) : base.ReadInt32();
        }

        public int[] ReadInt32s(int count)
        {
            var array = new int[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadInt32();
            }

            return array;
        }

        public override long ReadInt64()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadInt64()) : base.ReadInt64();
        }

        public long[] ReadInt64s(int count)
        {
            var array = new long[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadInt64();
            }

            return array;
        }

        public float[] ReadSingles(int count)
        {
            var array = new float[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadSingle();
            }

            return array;
        }

        public override uint ReadUInt32()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadUInt32()) : base.ReadUInt32();
        }

        public uint[] ReadUInt32s(int count)
        {
            var array = new uint[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadUInt32();
            }

            return array;
        }

        public override ulong ReadUInt64()
        {
            return SwapBytes ? EndiannessHelper.Swap(base.ReadUInt64()) : base.ReadUInt64();
        }

        public ulong[] ReadUInt64s(int count)
        {
            var array = new ulong[count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = ReadUInt64();
            }

            return array;
        }

        public string ReadString(StringBinaryFormat format, int fixedLength = -1)
        {
            mStringBuilder.Clear();

            switch (format)
            {
                case StringBinaryFormat.NullTerminated:
                    {
                        byte b;
                        while ((b = ReadByte()) != 0)
                            mStringBuilder.Append((char)b);
                    }
                    break;

                case StringBinaryFormat.FixedLength:
                    {
                        if (fixedLength == -1)
                            throw new ArgumentException("Invalid fixed length specified");

                        byte b;
                        var terminated = false;
                        for (var i = 0; i < fixedLength; i++)
                        {
                            b = ReadByte();
                            if ( b == 0 )
                                terminated = true;

                            if ( !terminated )
                                mStringBuilder.Append( ( char ) b );
                        }
                    }
                    break;

                case StringBinaryFormat.PrefixedLength8:
                    {
                        byte length = ReadByte();
                        for (var i = 0; i < length; i++)
                            mStringBuilder.Append((char)ReadByte());
                    }
                    break;

                case StringBinaryFormat.PrefixedLength16:
                    {
                        ushort length = ReadUInt16();
                        for (var i = 0; i < length; i++)
                            mStringBuilder.Append((char)ReadByte());
                    }
                    break;

                case StringBinaryFormat.PrefixedLength32:
                    {
                        uint length = ReadUInt32();
                        for (var i = 0; i < length; i++)
                            mStringBuilder.Append((char)ReadByte());
                    }
                    break;

                default:
                    throw new ArgumentException("Unknown string format", nameof(format));
            }

            return mStringBuilder.ToString();
        }

        public string ReadStringAtOffset( long offset, StringBinaryFormat format, int fixedLength = -1 )
        {
            if ( offset == 0 )
                return null;

            string str = null;
            ReadAtOffset( offset, () => str = ReadString( format, fixedLength ) );
            return str;
        }

        public string ReadStringOffset( StringBinaryFormat format = StringBinaryFormat.NullTerminated, int fixedLength = -1 )
        {
            var offset = ReadInt32();
            if ( offset == 0 )
                return null;

            return ReadStringAtOffset( offset, format, fixedLength );
        }

        public string[] ReadStrings(int count, StringBinaryFormat format, int fixedLength = -1)
        {
            var value = new string[count];
            for (var i = 0; i < value.Length; i++)
                value[i] = ReadString(format, fixedLength);

            return value;
        }

        public string[] ReadStringsAtOffset( long offset, int count, StringBinaryFormat format, int fixedLength = -1 )
        {
            string[] str = null;
            ReadAtOffset( offset, () => str = ReadStrings( count, format, fixedLength ) );
            return str;
        }

        public T ReadObject<T>( object context = null ) where T : ISerializableObject, new()
        {
            //if ( !mObjectLookup.TryGetValue( Position, out var obj ) )
            //{
            //    obj = new T
            //    {
            //        SourceFilePath   = FileName,
            //        SourceOffset     = Position,
            //        SourceEndianness = mEndianness
            //    };
            //    mObjectLookup[Position] = obj;
            //    ((T)obj).Read( this, context );
            //}

            //return (T)obj;

            var obj = new T
            {
                SourceFilePath   = FileName,
                SourceOffset     = Position,
                SourceEndianness = mEndianness
            };

            ( ( T )obj ).Read( this, context );
            return obj;
        }

        /// <summary>
        /// Reads an object of type <typeparamref name="T"/> from the given relative offset if it is not in the object cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offset"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public T ReadObjectAtOffset<T>( long offset, object context = null ) where T : ISerializableObject, new()
        {
            object obj = null;
            var effectiveOffset = offset + BaseOffset;

            if ( offset != 0 && !mObjectLookup.TryGetValue( effectiveOffset, out obj ) )
            {
                long current = Position;
                SeekBegin( effectiveOffset );
                obj = ReadObject<T>( context );
                SeekBegin( current );
                mObjectLookup[effectiveOffset] = obj;
            }

            return ( T ) obj;
        }

        /// <summary>
        /// Reads an object offset from the current stream and reads the object of type <typeparamref name="T"/> at the given offset, if it is not in the object cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public T ReadObjectOffset<T>( object context = null ) where T : ISerializableObject, new()
        {
            var offset = ReadInt32();
            if ( offset == 0 )
                return default( T );

            return ReadObjectAtOffset<T>( offset, context );
        }
    }
}
