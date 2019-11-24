using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locksmith.Data
{
    public interface IPatch
    {
        List<IPatchRecord> Records { get; }
        void AddRecord(int offset, int size, byte[] data);
        byte[] ToByteArray();
    }

    /// <summary>
    /// IPS Patch structure.  See http://zerosoft.zophar.net/ips.php for spec.
    /// </summary>
    public class IPSPatch : IPatch
    {
        private readonly byte[] PATCH_BYTES = new byte[] { 0x50, 0x41, 0x54, 0x43, 0x48 };
        private readonly byte[] EOF_BYTES = new byte[] { 0x45, 0x4f, 0x46 };

        public IPSPatch()
        {
            Records = new List<IPatchRecord>();
        }

        public List<IPatchRecord> Records { get; private set; }


        public void AddRecord(int offset, int size, byte[] data)
        {
            Records.Add(new IPSRecord(offset, size, data));
        }

        public byte[] ToByteArray()
        {
            // Format is "PATCH" + data + "EOF";
            IEnumerable<byte> patchBytes = PATCH_BYTES;

            Records.ForEach(r =>
                {

                    // 3 bytes for offset, 2 bytes for size, then data
                    byte[] offsetBytes = BitConverter.GetBytes(r.Offset);
                    byte[] sizeBytes = BitConverter.GetBytes(r.Size);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(offsetBytes);
                        Array.Reverse(sizeBytes);
                    }

                    patchBytes =
                           patchBytes.Concat(offsetBytes)
                                     .Concat(sizeBytes)
                                     .Concat(r.RecordData);
                }
            );

            patchBytes = patchBytes.Concat(EOF_BYTES);
            return patchBytes.ToArray();
        }
    }

    public interface IPatchRecord
    {
        int Offset { get; }
        int Size { get; }
        byte[] RecordData { get; }
    }

    public class IPSRecord : IPatchRecord
    {
        public IPSRecord(int offset, int size, byte[] data)
        {
            Offset = offset;
            Size = size;
            RecordData = data;
        }

        public int Offset { get; private set; }
        public int Size { get; private set; }
        public byte[] RecordData { get; private set; }
    }
}
