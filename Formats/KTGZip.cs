using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using G1Tool.IO;
using System;
using System.Linq;

namespace G1Tool.Formats
{
    class KTGZip
    {
        public static byte[] Decompress(byte[] file)
        {
            using (EndianBinaryReader r = new EndianBinaryReader(new MemoryStream(file), Endianness.Little))
            {
                uint splitSize = r.ReadUInt32();
                uint entryCount = r.ReadUInt32();
                uint uncompSize = r.ReadUInt32();

                uint[] splits = r.ReadUInt32s((int)entryCount);
                byte[] output = new byte[uncompSize];

                r.SeekBegin((r.Position + 0x7F) & ~0x7F); // Align

                using (EndianBinaryWriter w = new EndianBinaryWriter(new MemoryStream(output), Endianness.Little))
                {
                    for (int i = 0; i < entryCount; i++)
                    {
                        uint cur_comp = r.ReadUInt32();
                        if (i == entryCount - 1)
                        {
                            if (cur_comp != splits[i] - 4)
                                w.Write(splits[i]);
                            else
                            {
                                using (GZipStream deflate = new GZipStream(new MemoryStream(r.ReadBytes((int)cur_comp)), CompressionMode.Decompress))
                                    deflate.CopyTo(w.BaseStream);
                            }
                        }
                        else
                        {
                            if (cur_comp == splits[i] - 4)
                            {
                                using (GZipStream deflate = new GZipStream(new MemoryStream(r.ReadBytes((int)cur_comp)), CompressionMode.Decompress))
                                    deflate.CopyTo(w.BaseStream);
                            }
                        }

                        r.SeekBegin(r.Position + 0x7F & ~0x7F); // Align
                    }
                }
                return output;
            }
        }
        public static byte[] Compress(byte[] file)
        {
            int splitSize = 0x10000;
            int partitionCount = ((file.Length - 1) / 0x10000) + 1;

            var partitionList = new List<byte[]>();
            using (var reader = new BinaryReader(new MemoryStream(file)))
            {
                for (int i = 0; i < partitionCount; i++)
                {
                        byte[] partition = ZLib.Compress(reader.ReadBytes(splitSize));
                        partitionList.Add(partition);
                }

            }
            using (var ms = new MemoryStream())
            using (var br = new BinaryWriter(ms))
            {
                // Header
                br.Write(splitSize);
                br.Write(partitionCount);
                br.Write(file.Length);  // Uncompressed file length
                for (int i = 0; i < partitionList.Count; i++)
                {
                    br.Write(partitionList[i].Length + 4);  // Entry size (partition size member + partition)
                }
                long currentOffset = br.BaseStream.Position;
                // Entries
                for (int i = 0; i < partitionList.Count; i++)
                {
                    br.BaseStream.Position = currentOffset + 0x7F & ~0x7F; // Aligning
                    br.Write(partitionList[i].Length);
                    br.BaseStream.Write(partitionList[i], 0x0, partitionList[i].Length);
                    currentOffset = br.BaseStream.Position;
                }
                ms.Capacity = (int)br.BaseStream.Position;   // MS doubles capacity every time buffer is exceeded
                return ms.GetBuffer();
            }
        }
    }
    // Credits: https://github.com/KillzXGaming/Switch-Toolbox
    public class ZLib
    {
        public static byte[] Compress(byte[] b, uint Position = 0)
        {
            var output = new MemoryStream();
            output.Write(new byte[] { 0x78, 0xDA }, 0, 2);

            using (var zipStream = new DeflateStream(output, CompressionMode.Compress, true))
                zipStream.Write(b, 0, b.Length);

            //Add this as it weirdly prevents the data getting corrupted
            //From https://github.com/IcySon55/Kuriimu/blob/f670c2719affc1eaef8b4c40e40985881247acc7/src/Kontract/Compression/ZLib.cs
            var adler = b.Aggregate(Tuple.Create(1, 0), (x, n) => Tuple.Create((x.Item1 + n) % 65521, (x.Item1 + x.Item2 + n) % 65521));
            output.Write(new[] { (byte)(adler.Item2 >> 8), (byte)adler.Item2, (byte)(adler.Item1 >> 8), (byte)adler.Item1 }, 0, 4);
            return output.ToArray();
        }


    }

}