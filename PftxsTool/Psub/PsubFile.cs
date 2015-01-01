﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using PftxTool;

namespace PftxsTool.Psub
{
    public class PsubFile
    {
        private const int MagicNumber = 0x42555350; // PSUB
        private readonly List<PsubFileIndex> _indices;

        public PsubFile()
        {
            _indices = new List<PsubFileIndex>();
        }

        public int EntryCount { get; set; }

        public IEnumerable<PsubFileIndex> Indices
        {
            get { return _indices; }
        }

        public static PsubFile ReadPsubFile(Stream input)
        {
            PsubFile psubFile = new PsubFile();
            psubFile.Read(input);
            return psubFile;
        }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            int magicNumber = reader.ReadInt32();
            EntryCount = reader.ReadInt32();
            for (int i = 0; i < EntryCount; i++)
            {
                PsubFileIndex index = new PsubFileIndex();
                index.Read(input);
                _indices.Add(index);
            }
            input.AlignRead(16);
            foreach (var index in Indices)
            {
                index.Data = reader.ReadBytes(index.Size);
            }
            input.AlignRead(16);
        }
    }
}
