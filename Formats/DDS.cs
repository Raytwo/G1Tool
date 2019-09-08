using G1Tool.IO;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.Textures.TextureFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1Tool.Formats
{
    public class DDS
    {
        public uint Size { get; private set; }
        /// <summary>
        /// Width (in pixel)
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Height (in pixel)
        /// </summary>
        public int Height { get; private set; }
        public int TextureSize
        {
            get
            {
                switch (InternalFormat)
                {
                    case InternalFormat.Rgba8:
                        return (Width * Height) * 4;
                    case InternalFormat.CompressedRgbS3tcDxt1Ext:
                        return (Width * Height / 2);
                    case InternalFormat.CompressedRgbaS3tcDxt5Ext:
                        return (Width * Height);
                    default:
                        return -1;
                }
            }
        }
        /// <summary>
        /// Depth of a volume texture (in pixel)
        /// </summary>
        public uint Depth { get; private set; }
        /// <summary>
        /// Number of mipmap levels
        /// </summary>
        public uint MipMapCount { get; set; }
        public uint PixelFormat { get; private set; }
        public InternalFormat InternalFormat { get; set; }

        public uint Caps { get; private set; }

        public Texture2D Texture { get; private set; }

        public enum DDSCAPS : uint
        {
            COMPLEX = 0x00000008,
            TEXTURE = 0x00001000,
            MIPMAP = 0x00400000
        }

        public DDS()
        {
            Texture = new Texture2D();
        }

        public void Read(string filename)
        {
            Read(new EndianBinaryReader(filename, Endianness.Little));
        }

        public void Read(EndianBinaryReader r)
        {
            if (r.ReadUInt32() != 0x20534444)
            {
                MessageBox.Show("This file is not a DirectDraw Surface.", "Invalid file");
                return;
            }

            Size = r.ReadUInt32();
            uint flags = r.ReadUInt32();

            Height = r.ReadInt32();
            Width = r.ReadInt32();
            uint pitch = r.ReadUInt32();
            Depth = r.ReadUInt32();
            MipMapCount = r.ReadUInt32();
            if (MipMapCount == 0)
                MipMapCount = 1;
            r.SeekCurrent(4 * 11); //Skip reserved
            uint[] pixelformat = r.ReadUInt32s(2);
            InternalFormat = GetInternalFormatForTextures(r.ReadString(StringBinaryFormat.FixedLength, 4));
            r.ReadUInt32s(5);
            Caps = r.ReadUInt32();
            uint burnes = r.ReadUInt32();
            uint[] unused = r.ReadUInt32s(3);

            if (TextureFormatTools.IsCompressed(InternalFormat))
                Texture.LoadImageData(Width, Height, r.ReadBytes(TextureSize), InternalFormat);
            else
                Texture.LoadImageData(Width, Height, r.ReadBytes(TextureSize), new TextureFormatUncompressed(PixelInternalFormat.Rgba8, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte));

        }

        public static InternalFormat GetInternalFormatForTextures(string value)
        {
            switch (value)
            {
                case "DXT1":
                    return InternalFormat.CompressedRgbS3tcDxt1Ext;
                case "DXT5":
                    return InternalFormat.CompressedRgbaS3tcDxt5Ext;
                default:
                    return InternalFormat.Rgba8;
            }
        }
    }
}
