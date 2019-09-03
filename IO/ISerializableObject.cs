namespace G1Tool.IO
{
    /// <summary>
    /// Common interface for all objects that can be serialized from and to a stream.
    /// </summary>
    public interface ISerializableObject
    {
        /// <summary>
        /// Path to the file from which the object was read.
        /// </summary>
        string SourceFilePath { get; set; }

        /// <summary>
        /// Offset from which the object was read.
        /// </summary>
        long SourceOffset { get; set; }

        /// <summary>
        /// Endiannes in which the object was read.
        /// </summary>
        Endianness SourceEndianness { get; set; }

        /// <summary>
        /// Reads the object from a stream using the provided reader.
        /// </summary>
        /// <param name="reader">The reader to read with.</param>
        /// <param name="context">Custom context data to be used during reading.</param>
        void Read( EndianBinaryReader reader, object context = null );

        /// <summary>
        /// Writes the object to a stream using the provided writer.
        /// </summary>
        /// <param name="writer">The writer to writer with.</param>
        /// <param name="context">Custom context data to be used during writing.</param>
        void Write( EndianBinaryWriter writer, object context = null );
    }
}
