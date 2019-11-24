using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Locksmith.Data
{
    public interface IPatchManager
    {
        /// <summary>
        /// Compares the base ROM to the modified ROM to create an patch from the delta.
        /// </summary>
        /// <param name="BaseRom">Unmodified ROM file from disk</param>
        /// <param name="ModifiedRom">ROM file with in-memory edits</param>
        /// <returns>Patch build from the patch manager implemented</returns>
        IPatch CreatePatch(Romulus.Nes.Rom BaseRom, Romulus.Nes.Rom ModifiedRom);

        /// <summary>
        /// File filter used in the Save File Dialog window.
        /// </summary>
        String SaveFileFilter { get; }

        /// <summary>
        /// Save the patch to disk.
        /// </summary>
        /// <param name="patch">Patch created from CreatePatch()</param>
        /// <param name="fileName">File name to save on disk.</param>
        void Save(IPatch patch, String fileName);

        /// <summary>
        /// Load an IPS patch from disk
        /// </summary>
        /// <param name="fileName">File name of the IPS patch.</param>
        /// <returns></returns>
        IPatch Load(String fileName);

        /// <summary>
        /// Applies an IPS patch to a Romulus ROM file
        /// </summary>
        /// <param name="BaseRom"></param>
        /// <param name="Patch"></param>
        void ApplyIPSPatch(Romulus.Nes.Rom BaseRom, IPatch Patch);
    }

    public class IPSManager : IPatchManager
    {
        public String SaveFileFilter { get { return "IPS File|*.ips"; } }

        /// <summary>
        /// Compares the base ROM to the modified ROM to create an IPS patch
        /// </summary>
        /// <param name="BaseRom">Unmodified ROM file from disk</param>
        /// <param name="ModifiedRom">ROM file with in-memory edits</param>
        /// <returns>IPS Patch records</returns>
        /// <see cref="http://zerosoft.zophar.net/ips.php"/>
        public IPatch CreatePatch(Romulus.Nes.Rom BaseRom, Romulus.Nes.Rom ModifiedRom)
        {
            /*
             * Compare each data element one by one.
             * If a difference is found, track mark the start position.
             * Continue until the data in the base and modified ROM files are the same and mark the end position.
             * Add a new line entry to the IPS patch marking start position, length, and changes.
             * 
             * Continue checking for changes until EOF.
             * 
             * Add header information to the patch.
             */
            IPatch patch = new IPSPatch();

            int romLength = BaseRom.data.Length;

            for (int i = 0; i < romLength; i++)
            {
                // Difference found.  Find where they stop being different.
                if (BaseRom.data[i] != ModifiedRom.data[i])
                {
                    int startingOffset = i;
                    int endingOffset = i;
                    do
                    {
                        endingOffset = ++i;
                    } while (i < romLength && BaseRom.data[i] != ModifiedRom.data[i]);
                    int recSize = endingOffset - startingOffset;
                    patch.AddRecord(startingOffset, recSize,
                        ModifiedRom.data.Skip(startingOffset).Take(recSize).ToArray());
                }
            }

            return patch;
        }

        public void Save(IPatch patch, String fileName)
        {
            File.WriteAllBytes(fileName, patch.ToByteArray());
        }

        public IPatch Load(String fileName)
        {
            byte[] ipsFile = File.ReadAllBytes(fileName);
            
            // Ignore the PATCH and EOF.
            // 3 bytes - ROM offset
            // 2 bytes - size
            // [size] bytes - record data

            int ipsPointer = 5; // Skip "PATCH" at BOF
            IPatch patch = new IPSPatch();
            while (ipsPointer < ipsFile.Length - 3) // Go until you hit EOF
            {
                int offset, size; byte[] data;
                offset = BitConverter.ToInt32(ipsFile.Skip(ipsPointer).Take(3).ToArray(), 0);
                size = BitConverter.ToInt32(ipsFile.Skip(ipsPointer + 3).Take(2).ToArray(), 0);
                data = ipsFile.Skip(ipsPointer + 5).Take(size).ToArray();
                patch.AddRecord(offset, size, data);
                ipsPointer += (5 + size);
            }
            return patch;
        }

        /// <summary>
        /// Applies an IPS patch to the ROM
        /// </summary>
        public void ApplyIPSPatch(Romulus.Nes.Rom BaseRom, IPatch Patch)
        {
            /*
             * Validate the IPS patch.  Format is:
             *    PATCH
             *    Records
             *    EOF
             * 
             * For each record, apply the bytes to the base ROM at the record's offset.
             */
            Patch.Records.ForEach(r =>
                    Array.Copy(r.RecordData, 0, BaseRom.data, r.Offset, r.Size)
            );
            return;
        }
    }
}

