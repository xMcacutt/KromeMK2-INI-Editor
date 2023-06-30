using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DamienG.Security.Cryptography;

namespace Ty2INIEditor
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

        public void GenerateEntryNameTable(string[] files)
        {
            // Calculate the total length of the name table
            EntryNameTableLength = files.Sum(f => 1 + Path.GetFileName(f).Length) + 1;
            // Create a memory stream to hold the byte array
            using (MemoryStream stream = new MemoryStream(EntryNameTableLength))
            {
                foreach (string file in files)
                {
                    // Get the file name as a byte array in UTF-8 encoding
                    byte[] nameBytes = Encoding.ASCII.GetBytes(Path.GetFileName(file));

                    // Write the 0x0 byte separator
                    stream.WriteByte(0x0);

                    // Write the file name to the stream
                    EntryNameTableOffsets.Add((int)stream.Position);
                    stream.Write(nameBytes, 0, nameBytes.Length);
                }
                stream.WriteByte(0x0);

                // Get the byte array from the stream
                EntryNameTable = stream.ToArray();
            }
        }

        public void GenerateAddendumTable(string[] files, string inputDir)
        {
            List<string> aliases = new List<string>();
            foreach(string file in files)
            {
                string relativePath = Utility.GetRelativePath(inputDir, file);
                if (relativePath != Path.GetFileName(file))
                {
                    if(FileExtAlias.Aliases.TryGetValue(Path.GetExtension(file).ToLower(), out string aliasExt))
                    {
                        if (aliasExt == ".m3d" && FileExtAlias.MdlExceptions.Contains(Path.GetFileNameWithoutExtension(file))) aliasExt = ".FBX";
                        relativePath = relativePath.Replace(Path.GetExtension(file), aliasExt);
                    }
                    aliases.Add(relativePath);
                }
            }
            AddendumTableLegnth = aliases.Sum(p => 1 + p.Length) + 1;

            using (MemoryStream stream = new MemoryStream(AddendumTableLegnth))
            {
                string[] aliasesArray = aliases.ToArray();
                foreach (string path in aliases)
                {
                    // Get the file name as a byte array in UTF-8 encoding
                    byte[] pathBytes = Encoding.ASCII.GetBytes(path);

                    // Write the 0x0 byte separator
                    stream.WriteByte(0x0);

                    // Write the file name to the stream
                    AddendumTableOffsets.Add((int)stream.Position);
                    stream.Write(pathBytes, 0, pathBytes.Length);
                }
                stream.WriteByte(0x0);

                // Get the byte array from the stream
                AddendumTable = stream.ToArray();
            }
        }

        public void GenerateFileData(string[] files)
        {
            MemoryStream combinedStream = new MemoryStream();
            foreach (string file in files)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    FileDataOffsets.Add((int)combinedStream.Position);
                    fileStream.CopyTo(combinedStream);

                    int bytesToPad = 16 - (int)combinedStream.Position % 16;
                    combinedStream.Write(new byte[bytesToPad], 0, bytesToPad);
                }
            }
            FileData = combinedStream.ToArray();
        }

        public void Repack(string inputDir, string outputDir)
        {
            Signature = "RKV2";
            string[] files = Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories).Where(x => !x.EndsWith(".mk2proj", StringComparison.CurrentCultureIgnoreCase)).ToArray();
            EntryCount = files.Length;
            GenerateEntryNameTable(files);
            GenerateAddendumTable(files, inputDir);
            GenerateFileData(files);
            List<string> entryStrings = new List<string>();
            DateTimeOffset now = DateTime.Now;
            int timestamp = (int)now.ToUnixTimeSeconds();
            List<string> addenda = new List<string>();
            foreach (string file in files)
            {
                var crc32 = new Crc32();
                int fileCrc32;
                string entryName = Path.GetFileName(file);
                int stringIndex = Array.IndexOf(files, file);
                FileInfo fileInfo = new FileInfo(file);
                using (var fs = File.Open(file, FileMode.Open)) fileCrc32 = BitConverter.ToInt32(crc32.ComputeHash(fs).Reverse().ToArray(), 0);
                Entry entry = new Entry()
                {
                    Name = entryName,
                    Size = (int)fileInfo.Length,
                    crc32eth = fileCrc32,
                    NameTableOffset = EntryNameTableOffsets[stringIndex],
                    Offset = FileDataOffsets[stringIndex] + 0x80
                };
                if (entryStrings.Contains(Path.GetFileNameWithoutExtension(entryName)))
                {
                    entry.GroupingReference = Entries.FindLast(x => Path.GetFileNameWithoutExtension(x.Name) == Path.GetFileNameWithoutExtension(entryName)).NameTableOffset;
                }
                Entries.Add(entry);
                entryStrings.Add(Path.GetFileNameWithoutExtension(entryName));

                string relativePath = Utility.GetRelativePath(inputDir, file);
                if (relativePath != Path.GetFileName(file))
                {
                    if(FileExtAlias.Aliases.TryGetValue(Path.GetExtension(file).ToLower(), out string aliasExt))
                    {
                        if (aliasExt == ".m3d" && FileExtAlias.MdlExceptions.Contains(Path.GetFileNameWithoutExtension(file))) aliasExt = ".FBX";
                        relativePath = relativePath.Replace(Path.GetExtension(file), aliasExt);
                    }
                    addenda.Add(file);
                    Addendum addendum = new Addendum()
                    {
                        Path = relativePath,
                        AddendumTableOffset = AddendumTableOffsets[addenda.Count - 1],
                        Entry = entry,
                        EntryNameTableOffset = entry.NameTableOffset,
                        TimeStamp = timestamp
                    };
                    Addenda.Add(addendum);
                }
                AddendumCount = addenda.Count;
            }
            MetadataTableOffset = 0x80 + FileData.Length;
            MetaDataTableLength = (ENTRY_SIZE * EntryCount) + (ADDENDUM_SIZE * AddendumCount) + EntryNameTable.Length + AddendumTable.Length;
            MetaDataTableLength += 16 - (int)MetaDataTableLength % 16;
            EntryNameTableOffset = MetadataTableOffset + MetaDataTableLength;
            AddendumTableOffset = EntryNameTableOffset + EntryNameTableLength;

            byte[] zeroInt = new byte[] { 0, 0, 0, 0 };
            using (var rkv = File.Create(outputDir + "\\" + Path.GetFileName(inputDir) + ".rkv"))
            {
                rkv.Write(Encoding.ASCII.GetBytes(Signature), 0, 4);
                rkv.Write(BitConverter.GetBytes(EntryCount), 0, 4);
                rkv.Write(BitConverter.GetBytes(EntryNameTableLength), 0, 4);
                rkv.Write(BitConverter.GetBytes(AddendumCount), 0, 4);
                rkv.Write(BitConverter.GetBytes(AddendumTableLegnth), 0, 4);
                rkv.Write(BitConverter.GetBytes(MetadataTableOffset), 0, 4);
                rkv.Write(BitConverter.GetBytes(MetaDataTableLength), 0, 4);
                rkv.Write(new byte[] { 0x0F, 0x07, 0x0F, 0x07, 0x30, 0x03 }, 0, 6);
                rkv.Write(Enumerable.Repeat((byte)0x0, 0x5E).ToArray(), 0, 0x5E);
                rkv.Write(FileData, 0, FileData.Length);
                int entryIndex = 0;
                Entries.Sort((a1, a2) => string.Compare(a1.Name, a2.Name, StringComparison.OrdinalIgnoreCase));
                foreach(Entry entry in Entries)
                {
                    rkv.Write(BitConverter.GetBytes(entry.NameTableOffset), 0, 4);
                    rkv.Write(BitConverter.GetBytes(entry.GroupingReference), 0, 4);
                    rkv.Write(BitConverter.GetBytes(entry.Size), 0, 4);
                    rkv.Write(BitConverter.GetBytes(entry.Offset), 0, 4);
                    rkv.Write(BitConverter.GetBytes(entry.crc32eth), 0, 4);
                    entryIndex++;
                }
                rkv.Write(EntryNameTable, 0, EntryNameTable.Length);
                int addendumIndex = 0;
                Addenda.Sort((a1, a2) => string.Compare(a1.Path, a2.Path, StringComparison.OrdinalIgnoreCase));
                foreach (Addendum addendum in Addenda)
                {
                    rkv.Write(BitConverter.GetBytes(addendum.AddendumTableOffset), 0, 4);
                    rkv.Write(zeroInt, 0, 4);
                    rkv.Write(BitConverter.GetBytes(addendum.TimeStamp), 0, 4);
                    rkv.Write(BitConverter.GetBytes(addendum.EntryNameTableOffset), 0, 4);
                    addendumIndex++;
                }
                rkv.Write(AddendumTable, 0, AddendumTable.Length);
                int bytesToPad = 16 - (int)rkv.Position % 16;
                rkv.Write(new byte[bytesToPad], 0, bytesToPad);
            }
        }
    }
}
