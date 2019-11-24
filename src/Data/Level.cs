using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romulus.Nes;
using Locksmith.Data;

namespace Locksmith.Data
{
    public enum KeyState
    {
        Normal = 0x01,
        HiddenInBlock = 0x40,
        Hidden = 0x80
    }

    public class Level
    {
        public const int keyStatusOffset = 4;
        public const int doorPositionOffset = 5;
        public const int keyPositionOffset = 6;
        public const int playerStartOffset = 7;
        public const int spawn1Offset = 8;
        public const int spawn2Offset = 9;
        public const int itemSetOffset = 10;

        
        public int LevelNumber { get; private set; }
        public byte PlayerStart { get; set; }
        public SolKeyLevelElement Exit { get; set; }

        public SolKeyLevelElement Key { get; private set; }
        public KeyState KeyStatus { get; private set; }
        public List<SolKeyLevelElement> ItemsList { get; private set; }
        public List<SolKeyLevelElement> EnemiesList { get; private set; }
        public byte EnemySpawnRate { get; private set; }
        
        /// <summary>
        /// Array listing the set magic blocks
        /// </summary>
        public byte[] MagicBlocks { get; private set; }

        /// <summary>
        /// Array listing the set solid blocks
        /// </summary>
        public byte[] SolidBlocks { get; private set; }
        public SolKeyLevelElement SpawnPos1 { get; private set; }
        public SolKeyLevelElement SpawnPos2 { get; private set; }
        public int TileSetIndex { get; private set; }
        public bool HasConstellation { get { return Constellation != null; } }
        public SolKeyLevelElement Constellation { get; private set; }

        /// <summary>
        /// The last value that must be placed in the item list if no constellation is present.
        /// </summary>
        public byte ItemTerminator { get; private set; }

        // Each level may have a different background tile set and background palette
        public PatternTable BackgroundPatterns { get; private set; }
        public CompositePalette BackgroundPalette { get; private set; }
        private SolRom SolKeyRom { get; set; }

        public Level(int LevelNum, SolRom Rom, int PaletteIndex)
        {
            this.LevelNumber = LevelNum;
            this.SolKeyRom = Rom;
            this.ItemsList = new List<SolKeyLevelElement>();
            this.EnemiesList = new List<SolKeyLevelElement>();

            // Load the item list
            LoadItemList(Rom);

            this.BackgroundPatterns = Rom.bgPatterns[TileSetIndex];
            // Load the enemy list
            LoadEnemyList();
            // Load the magic blocks
            MagicBlocks = new byte[24];
            LoadBlocksFromROM(MagicBlocks, false, Rom);
            // Load the solid blocks
            SolidBlocks = new byte[24];
            LoadBlocksFromROM(SolidBlocks, true, Rom);
        }

        public bool ValidateLevel()
        {
            // Each level must have a start position, key, and door in valid spots.
            return true;
        }

        private void LoadBlocksFromROM(byte[] blockSet, bool SolidBlocks, SolRom SolKeyRom)
        {
            // Start at the ROM's block offset + (levelNum - 1) * 2 * length.
            // The next 24 bytes define the preloaded magic blocks
            int blockOffset = SolidBlocks ? SolKeyRom.LevelBlockLength : 0;
            pHRom startPos = SolKeyRom.LevelBlockOffset + (this.LevelNumber - 1) * 2 * SolKeyRom.LevelBlockLength + blockOffset;
            pHRom endPos = startPos + SolKeyRom.LevelBlockLength;
            int blockIndex = 0;
            for (int romOffset = startPos.Offset; romOffset < endPos.Offset; romOffset++)
            {
                blockSet[blockIndex++] = SolKeyRom.data[romOffset];
            }
        }

        private void LoadEnemyList()
        {
            byte[] EnemyArray = GetEnemyArrayFromRom();

            // Spawn rate
            int i = 0;
            this.EnemySpawnRate = EnemyArray[i++];

            // Next pairs are enemy index/position pairs
            while (!SolRom.IsEnemyDelimiter(EnemyArray[i]))
            {
                EnemiesList.Add(new SolKeyLevelElement(ElementTypes.Enemy, EnemyArray[i], EnemyArray[i++]));
            }
        }

        private byte[] GetEnemyArrayFromRom()
        {
            return SolKeyRom.GetEnemyData(LevelNumber).ToArray();
        }

        /// <summary>
        /// Loads the ROM data into a the structured Level object
        /// </summary>
        private void LoadItemList(SolRom SolKeyRom)
        {
            // In the ROM, the "item" list also has positions of the player start, exit,
            // enemy spawns, zodiac painting, and background tile set.
            // Load all of these here, but only add actual items to the item list.
            byte[] ItemArray = GetItemArrayFromROM(SolKeyRom);
            this.SpawnPos1 = new SolKeyLevelElement(ElementTypes.SpawnPoint, 0x05, ItemArray[spawn1Offset]);
            this.SpawnPos2 = new SolKeyLevelElement(ElementTypes.SpawnPoint, 0x05, ItemArray[spawn2Offset]);
            this.PlayerStart = ItemArray[playerStartOffset];
            this.Key = new SolKeyLevelElement(ElementTypes.Key, 0x06, ItemArray[keyPositionOffset]);
            this.Exit = new SolKeyLevelElement(ElementTypes.Exit, 0x02, ItemArray[doorPositionOffset]);
            
            // the last element is either the position of the zodiac or the item delimiter
            byte patternByte = ItemArray[ItemArray.Length - 1];
            if (!SolRom.IsItemDelimiter(patternByte))
            {
                // if the last item was not the delimiter (wasn't like 0xe(n)), get the byte before
                patternByte = ItemArray[ItemArray.Length - 2];
            }
            this.ItemTerminator = patternByte;
            TileSetIndex = 0;
            if ((patternByte & 0x04) == 4)
                TileSetIndex = 1;
            else if ((patternByte & 0x08) == 8)
                TileSetIndex = 2;

            // Iterate through the rest of the item array to load items into the level's collection
            for (int i = itemSetOffset; i < ItemArray.Length; i++)
            {
                byte itemType = ItemArray[i];
                int repeatCount = 0;
                if (!SolRom.IsItemDelimiter(itemType) && !SolRom.IsItemZodiac(itemType))
                {
                    // Special rule for 0xC(n)
                    // 0xC(n) means that the next item will repeat n times.
                    // Example: C1 18 45 A5
                    //    The above set means that item 18 (bell) will be at position 45 and A5
                    // Commentary:  This is a stupid trick to save few bytes in the ROM, but back in 1987 it was necessary.
                    if (SolRom.IsItemRepeater(itemType))
                    {
                        repeatCount = itemType ^ SolRom.itemRepeat;
                        itemType = ItemArray[++i];
                    }
                    do
                    {
                        this.ItemsList.Add(new SolKeyLevelElement(ElementTypes.Item, itemType, ItemArray[++i]));
                        repeatCount--;
                    } while (repeatCount >= 0);
                }
                else
                {
                    // Does the level have a constellation icon?
                    // The constellation or the level end byte will be the last byte(s) 
                    if (SolRom.IsItemZodiac(itemType))
                    {
                        this.Constellation = new SolKeyLevelElement(ElementTypes.Item, itemType, ItemArray[++i]);
                    }
                }
            }
        }

        private byte[] GetItemArrayFromROM(SolRom SolKeyRom)
        {
            return SolKeyRom.GetItemData(LevelNumber).ToArray();
        }


        public void RemoveItemAtPosition(byte position)
        {
            SolKeyLevelElement itemToRemove = ItemsList.FirstOrDefault(i => i.Position == position);
            if (itemToRemove != null)
                ItemsList.Remove(itemToRemove);
        }

        public void AddItemAtPosition(SolKeyLevelElement item, byte position)
        {
            // DON'T ADD THE KEY OR THE EXIT TO THE ITEMS LIST
            // They are added separately in the level data
            if (item.ElementType == ElementTypes.Item)
            {
                item.Position = position;
                ItemsList.Add(item);
            }
        }

        /// <summary>
        /// Sets or unsets a magic (breakable) block at the position
        /// </summary>
        /// <param name="newPos"></param>
        public void ToggleMagicBlock(byte newPos)
        {
            ToggleBlock(this.MagicBlocks, newPos);
            ToggleBlock(this.SolidBlocks, newPos, false);
        }

        /// <summary>
        /// Sets or unsets a solid (unbreakable) block at the position
        /// </summary>
        /// <param name="newPos"></param>
        public void ToggleSolidBlock(byte newPos)
        {
            ToggleBlock(this.SolidBlocks, newPos);
            ToggleBlock(this.MagicBlocks, newPos, false);
        }

        #region Block Toggling Helper Functions
        private int GetBlockIndex(byte position)
        {
            // Translate the position byte into the index of the byte containing the block data
            int xPos = Utility.GetXPosition(position);
            int yPos = Utility.GetYPosition(position);
            return (yPos * 2) + (xPos < 8 ? -1 : 0) - 1;
        }

        private int GetBitIndex(byte position)
        {
            int xPos = Utility.GetXPosition(position);
            return (xPos < 8 ? xPos : xPos - 8);
        }

        /// <summary>
        /// Forces the state of a single block to true or false
        /// </summary>
        /// <param name="BlockList"></param>
        /// <param name="blockIndex"></param>
        /// <param name="bitToFlip"></param>
        /// <param name="forceState"></param>
        private void ToggleBlock(byte[] BlockList, int blockIndex, int bitToFlip, bool forceState)
        {
            // Set the bitToFlip-th bit of that set of blocks
            // lower number means a more significant bit.
            // ex. Flipping the 0th bit means changing 0000 0000 to 1000 0000
            // Flipping the 7th bit changes 0000 0000 to 0000 0010
            byte blockByte = BlockList[blockIndex];
            if (forceState)
            {
                // Force the bit to true
                // Easy, just bitwise OR with 0x80 right shifted bitToFlip times
                // Example: Forcing bit 6 to true on 1010 0011 -> 1010 0011 | 0000 0100 -> 1010 0111
                blockByte |= (byte)(0x80 >> bitToFlip);
            }
            else
            {
                // Force the bit to false
                // Need to invert what we're flipping and then bitwise AND
                // Example: Forcing bit 7 to false on 1010 0011 -> 1010 0011 & ~(0000 0010) ->
                //              1010 0011 & 1111 1101 -> 1010 0001
                blockByte &= (byte)(~(0x80 >> bitToFlip));
            }

            BlockList[blockIndex] = blockByte;
        }

        /// <summary>
        /// Returns true if the block is set.  Returns false otherwise.
        /// </summary>
        /// <param name="BlockList"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool GetCurrentBlockState(byte[] BlockList, byte position)
        {
            int blockIndex = GetBlockIndex(position);
            int bitToFlip = GetBitIndex(position);
            return (BlockList[blockIndex] & (byte)(0x80 >> bitToFlip)) == 0 ? false : true;
        }

        /// <summary>
        /// Flips the state of a single block
        /// </summary>
        /// <param name="BlockList"></param>
        /// <param name="position"></param>
        private void ToggleBlock(byte[] BlockList, byte position)
        {
            bool isBitSet = GetCurrentBlockState(BlockList, position);
            ToggleBlock(BlockList, GetBlockIndex(position), GetBitIndex(position), !isBitSet);
        }

        /// <summary>
        /// Forces a block to be set or unset.
        /// </summary>
        /// <param name="BlockList"></param>
        /// <param name="position"></param>
        /// <param name="forceState"></param>
        private void ToggleBlock(byte[] BlockList, byte position, bool forceState)
        {
            ToggleBlock(BlockList, GetBlockIndex(position), GetBitIndex(position), forceState);
        }
        #endregion

        /// <summary>
        /// Gets the key-value pairs of items and their count in the level.
        /// </summary>
        /// <returns></returns>
        public Dictionary<byte, int> GetItemCounts()
        {
            Dictionary<byte, int> counts = new Dictionary<byte, int>();
            ItemsList.ForEach(i =>
            {
                if (!counts.ContainsKey(i.ElementIndex))
                    counts.Add(i.ElementIndex, 0);
                counts[i.ElementIndex]++;
            });
            return counts;
        }

        internal List<Byte> GetItemListOutput()
        {
            List<Byte> outputBytes = new List<Byte>();

            Dictionary<byte, int> ItemCounts = GetItemCounts();
            Dictionary<byte, int>.Enumerator dictEnum = ItemCounts.GetEnumerator();
            while (dictEnum.MoveNext())
            {
                // Figure out what should be compressed by getting the number of elements of each type.
                // Any count more than one should be compressed.
                // Example: 3 bells at pos 94, 77, and 32 would normally be "18 94 18 77 18 32"
                // Instead the output will be "C2 18 94 77 32"
                // "C" signifies repeat, number + 1 is how many times it is repeated, and the next number + 1 bytes are the positions

                List<SolKeyLevelElement> itemElements = ItemsList.Where(i => i.ElementIndex == dictEnum.Current.Key).ToList();
                if (dictEnum.Current.Value == 1)
                {
                    // Only one?  Add the index and the position
                    outputBytes.Add(itemElements.Single().ElementIndex);
                    outputBytes.Add(itemElements.Single().Position);
                }
                else
                {
                    // More than one?  Compress.
                    outputBytes.Add((byte)(0xC0 | (dictEnum.Current.Value - 1)));
                    outputBytes.Add(itemElements[0].ElementIndex);
                    itemElements.ForEach(i => outputBytes.Add(i.Position));
                }
            }

            // Add either the constelation and its position or the end byte
            if (HasConstellation)
            {
                outputBytes.Add(Constellation.ElementIndex);
                outputBytes.Add(Constellation.Position);
            }
            else
                outputBytes.Add(ItemTerminator);

            return outputBytes;
        }
    }
}
