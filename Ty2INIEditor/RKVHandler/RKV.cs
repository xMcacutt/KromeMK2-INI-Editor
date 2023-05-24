using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DamienG.Security.Cryptography;
using System.Security.Cryptography;

namespace RKV2_Tools
{
    internal class RKV
    {
        private const int ENTRY_SIZE = 0x14;
        private const int ADDENDUM_SIZE = 0x10;

        public byte[] Data;
        public List<Entry> Entries = new List<Entry>();
        public List<Addendum> Addenda = new List<Addendum>();
        public List<int> EntryNameTableOffsets = new List<int>();
        public List<int> AddendumTableOffsets = new List<int>();
        public List<int> FileDataOffsets = new List<int>();
        public string Signature;
        public int EntryCount;
        public int EntryNameTableLength;
        public int EntryNameTableOffset;
        public int AddendumCount;
        public int AddendumDataOffset;
        public int AddendumTableLegnth;
        public int AddendumTableOffset;
        public int MetadataTableOffset;
        public int MetaDataTableLength;
        public byte[] EntryNameTable;
        public byte[] AddendumTable;
        public byte[] FileData;

        public void GenerateEntryTable(string file)
        {
            // Calculate the total length of the name table
            EntryNameTableLength = 2 + Path.GetFileName(file).Length;
            // Create a memory stream to hold the byte array
            using (MemoryStream stream = new MemoryStream(EntryNameTableLength))
            {
                // Get the file name as a byte array in UTF-8 encoding
                byte[] nameBytes = Encoding.UTF8.GetBytes(Path.GetFileName(file));

                // Write the 0x0 byte separator
                stream.WriteByte(0x0);

                // Write the file name to the stream
                EntryNameTableOffsets.Add((int)stream.Position);
                stream.Write(nameBytes, 0, nameBytes.Length);
                stream.WriteByte(0x0);

                // Get the byte array from the stream
                EntryNameTable = stream.ToArray();
            }
        }

        public void GenerateFileData(string file)
        {
            MemoryStream combinedStream = new MemoryStream();
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                FileDataOffsets.Add((int)combinedStream.Position);
                fileStream.CopyTo(combinedStream);
                int bytesToPad = 16 - (int)combinedStream.Position % 16;
                combinedStream.Write(new byte[bytesToPad], 0, bytesToPad);
            }
            FileData = combinedStream.ToArray();
        }

        public void Repack(string inputFilePath, string outputFilePath)
        {
            Signature = "RKV2";
            EntryCount = 1;
            GenerateEntryTable(inputFilePath);
            GenerateFileData(inputFilePath);
            var crc32 = new Crc32();
            int fileCrc32;
            string entryName = Path.GetFileName(inputFilePath);
            FileInfo fileInfo = new FileInfo(inputFilePath);
            using (var fs = File.Open(inputFilePath, FileMode.Open)) fileCrc32 = BitConverter.ToInt32(crc32.ComputeHash(fs), 0);
            Entry entry = new Entry()
            {
                Name = entryName,
                Size = (int)fileInfo.Length,
                crc32eth = fileCrc32,
                NameTableOffset = 1,
                Offset = 0x80
            };
            Entries.Add(entry);

            MetadataTableOffset = 0x80 + FileData.Length;
            MetaDataTableLength = (ENTRY_SIZE * EntryCount) + EntryNameTable.Length;
            EntryNameTableOffset = MetadataTableOffset + MetaDataTableLength;
            AddendumTableOffset = EntryNameTableOffset + EntryNameTableLength;
            using (var rkv = File.Create(outputFilePath))
            {
                rkv.Write(Encoding.UTF8.GetBytes(Signature), 0, 4);
                rkv.Write(BitConverter.GetBytes(EntryCount), 0, 4);
                rkv.Write(BitConverter.GetBytes(EntryNameTableLength), 0, 4);
                rkv.Write(new byte[4], 0, 4);
                rkv.Write(new byte[] {0x1, 0x0, 0x0, 0x0}, 0, 4);
                rkv.Write(BitConverter.GetBytes(MetadataTableOffset), 0, 4);
                rkv.Write(BitConverter.GetBytes(MetaDataTableLength), 0, 4);
                rkv.Write(new byte[] { 0x0F, 0x07, 0x0F, 0x07, 0x30, 0x03 }, 0, 6);
                rkv.Write(Enumerable.Repeat((byte)0x0, 0x5E).ToArray(), 0, 0x5E);
                rkv.Write(FileData, 0, FileData.Length);
                foreach (Entry e in Entries)
                {
                    rkv.Write(BitConverter.GetBytes(e.NameTableOffset), 0, 4);
                    rkv.Write(new byte[4], 0, 4);
                    rkv.Write(BitConverter.GetBytes(e.Size), 0, 4);
                    rkv.Write(BitConverter.GetBytes(e.Offset), 0, 4);
                    rkv.Write(BitConverter.GetBytes(e.crc32eth), 0, 4);
                }
                rkv.Write(EntryNameTable, 0, EntryNameTable.Length);
                int bytesToPad = 16 - (int)rkv.Position % 16;
                rkv.Write(new byte[bytesToPad], 0, bytesToPad);
            }
        }
    }
}
