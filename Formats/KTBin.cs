using System.Collections.Generic;
using System.IO;

namespace G1Tool.Formats
{
    public class KTBin
    {
        public List<byte[]> FileList { get; set; }
        public void Read(string path)
        {
            using (FileStream memoryStream = new FileStream(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                int numFiles = reader.ReadInt32();
                FileList = new List<byte[]>();
                for (int index = 0; index < numFiles; index++)
                {
                    reader.BaseStream.Position = 4 + (0x8 * index);
                    int fileOffset = reader.ReadInt32();
                    int fileSize = reader.ReadInt32();
                    byte[] buffer = new byte[fileSize];
                    reader.BaseStream.Position = fileOffset;
                    reader.Read(buffer, 0, fileSize);
                    FileList.Add(buffer);
                }
            }
        }
        public void Write(List<byte[]> fileList, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            using (var br = new BinaryWriter(fs))
            {
                // Header
                br.Write(fileList.Count);
                int headerSize = 0x4 + fileList.Count * 0x8;
                int fileOffset = headerSize;
                for (int i = 0; i < fileList.Count; i++)
                {
                    br.BaseStream.Position = 0x4 + i * 0x8;
                    br.Write(fileOffset);   // Offset
                    br.Write(fileList[i].Length);   // Size
                    br.BaseStream.Position = fileOffset;
                    br.BaseStream.Write(fileList[i], 0x0, fileList[i].Length);
                    fileOffset = (int)br.BaseStream.Position;
                }
            }
        }
    }
}
