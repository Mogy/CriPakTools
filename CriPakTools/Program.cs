using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace CriPakTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CriPakTools for DBX2_JP_MOD_TOOL\n");

            if (args.Length != 2)
            {
                Console.WriteLine("CriPakTool Usage:\n");
                Console.WriteLine("CriPakTool.exe IN_FILE EXTRACT_ME - Extracts a file.\n");
                return;
            }

            string cpk_name = args[0];

            CPK cpk = new CPK(new Tools());
            cpk.ReadCPK(cpk_name);

            BinaryReader oldFile = new BinaryReader(File.OpenRead(cpk_name));

            string extractMe = args[1];

            List<FileEntry> entries = null;

            entries = cpk.FileTable
                .Where(x => Regex.IsMatch(x.FileName.ToString(), extractMe))
                .ToList();

            if (entries.Count == 0)
            {
                Console.WriteLine("Cannot find " + extractMe + ".");
            }

            for (int i = 0; i < entries.Count; i++)
            {
                Console.CursorLeft = 0;
                Console.Write($"{i + 1}/{entries.Count}");

                if (!String.IsNullOrEmpty((string)entries[i].DirName))
                {
                    Directory.CreateDirectory(entries[i].DirName.ToString());
                }

                oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);
                string isComp = Encoding.ASCII.GetString(oldFile.ReadBytes(8));
                oldFile.BaseStream.Seek((long)entries[i].FileOffset, SeekOrigin.Begin);

                byte[] chunk = oldFile.ReadBytes(Int32.Parse(entries[i].FileSize.ToString()));
                if (isComp == "CRILAYLA")
                {
                    chunk = cpk.DecompressCRILAYLA(chunk, Int32.Parse(entries[i].ExtractSize.ToString()));
                }

                File.WriteAllBytes(((entries[i].DirName != null) ? entries[i].DirName + "/" : "") + entries[i].FileName.ToString(), chunk);
            }
        }
    }
}
