using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Romulus.Nes;
using Romulus.Nes.Mappers;

namespace Locksmith.Data
{
    public interface ISolRom
    {
        SolRom ROMData { get; }
        void Save(String Filename);

    }

    /// <summary>
    /// Represents a Solomon's Key rom
    /// </summary>
    public class SolRom:Rom<CxRomController>, ISolRom // This game uses the CNROM mapper.
    {
        public SolRom ROMData { get { return this; } }

        public static int MAX_LEVEL = 53;
        public static int MIN_LEVEL = 1;

        public const int LEVEL_PRINCESS = 49;
        public const int LEVEL_SOLOMON = 50;
        public const int LEVEL_HIDDEN = 51;
        public const int LEVEL_TIME = 52;
        public const int LEVEL_SPACE = 53;

        // Location of background tiles
        static pHRom bgPatternsOffset = (pHRom)0x9010;
        // Location of sprite tiles
        static pHRom spritePatternsOffset = (pHRom)0x8010;
        static int patternTableLength = 0x2000;
        // Palette location
        static pHRom paletteOffset = (pHRom)0x161a;

        // Pointer to level 1 enemy list is at val($5d31)val(5cfc).
        // Similar construct for items.
        // See documentation.
        static readonly pHRom enemyPointerLowBytesOffset = (pHRom)0x5cfc;
        static readonly pHRom enemyPointerHighBytesOffset = (pHRom)0x5d31;

        static readonly pHRom itemPointerLowBytesOffset = (pHRom)0x6a2c;
        static readonly pHRom itemPointerHighBytesOffset = (pHRom)0x6a61;

        static readonly pHRom levelBlockDataOffset = (pHRom)0x603C;
        static readonly int levelBlockDataLength = 24;

        // The index of the level's item data
        public int LevelItemAddress(int LevelNumber) { return GetAddressByPointers(itemPointerHighBytesOffset, itemPointerLowBytesOffset, LevelNumber).Offset; }
        public int LevelEnemyAddress(int LevelNumber) { return GetAddressByPointers(enemyPointerHighBytesOffset, enemyPointerLowBytesOffset, LevelNumber).Offset; }
        private readonly List<Level> Levels = new List<Level>();

        // Variable to load tiles to
        public List<PatternTable> bgPatterns { get; private set; }
        //public PatternTable bgPatterns { get; private set; }
        public PatternTable spritePatterns { get; private set; }
        public pHRom PaletteOffset { get { return paletteOffset; } }

        public pHRom LevelBlockOffset { get { return levelBlockDataOffset; } }
        public int LevelBlockLength { get { return levelBlockDataLength; } }

        // zodiac (0xF(n)) with a position and the delimiter (0xE(y)) are the last values in the item list
        // the lower byte (n or y, respectively) is used to determine which tileset the level uses.
        const int zodiacItem = 0xF0;
        const int itemDelimiter = 0xE0;
        public const int enemyDelimiter = 0x00;

        // in the item set, 0xC(n) where N is a hex number means the same item is used in the next (n) positions.
        public const int itemRepeat = 0xC0;


        // This Mapper object can be used to access data with pointers
        // or to convert beteen ROM offsets and NES addresses (a.k.a. pointers).
        public Mapper Mapper { get; private set; }

        public SolRom(string file)
            :base(file) {

            // Populate item and monster data
            PopulateItemNames();    
            PopulateEnemyNames();

            // Load our tiles
            // There are four different sets of bg patterns we'll use in different levels.
            // We only need to use three because the last one is used only for the title screen.
            bgPatterns = new List<PatternTable>();
            bgPatterns.Add(new PatternTable(true));
            bgPatterns[0].LoadWholeTable(data, bgPatternsOffset);

            bgPatterns.Add(new PatternTable(true));
            bgPatterns[1].LoadWholeTable(data, bgPatternsOffset + patternTableLength);

            bgPatterns.Add(new PatternTable(true));
            bgPatterns[2].LoadWholeTable(data, bgPatternsOffset + patternTableLength * 2);

            spritePatterns = new PatternTable(true);
            spritePatterns.LoadWholeTable(data, spritePatternsOffset);

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                Levels.Add(new Level(i + 1, this, 0));
            }

            // CNROM doesn't support PRG bank switching, so there is
            // only one Mapper object, which we get with GetMapper.
            //
            // Other mappers, such as MMC1, have bank switching. In
            // the case of MMC1, different banks can be accessed
            // by indexing Mappers, i.e. Mappers[0], Mappers[1], etc..
            this.Mapper = this.Mappers.GetMapper();

            
            // Example of mapper usage:
            
            // (Pointers needs to be in ROM address space, $8000-$FFFF.
            // this means you need to verify your pointers.)
            pCpu pointer = new pCpu(0x8765); 
            
            //  romFileOffset will have the location within the
            //  .NES file of the data 'pointer' points to.
            pHRom romFileOffset = Mapper.ToOffset(pointer);

            // We can also read objects from the ROM via pointer with the mapper
            var enemyPosition = Mapper.GetObject<sampleDataStructure>(pointer);
            // This way we don't have to manually de-reference pointers.
        }

        /// <summary>
        /// Saves the ROM data to the specified file
        /// </summary>
        /// <param name="Filename"></param>
        public void Save(String Filename)
        {
            BeforeSave();
            File.WriteAllBytes(Filename, this.data);
        }

        public override void CommitChanges()
        {
            base.CommitChanges();

            // Commit all level data to the rom
            Levels.ForEach(
                l => {
                    // Save the items (including start, key, and end pos)
                    SaveItemListToRom(l);
                    // Save the block positions
                    SaveBlocks(l);
                }
            );
        }

        private void SaveBlocks(Level l)
        {
            // Dump the magic blocks into the ROM data
            SaveBlocks(l, false, l.MagicBlocks);
            // Dump the solid blocks into the ROM data
            SaveBlocks(l, true, l.SolidBlocks);
        }

        private void SaveBlocks(Level l, bool SolidBlocks, byte[] blockSet)
        {
            // Start at the ROM's block offset + (levelNum - 1) * 2 * length.
            // The next 24 bytes define the blocks
            int blockOffset = SolidBlocks ? LevelBlockLength : 0;
            pHRom startPos = LevelBlockOffset + (l.LevelNumber - 1) * 2 * LevelBlockLength + blockOffset;
            pHRom endPos = startPos + LevelBlockLength;
            int blockIndex = 0;
            for (int romOffset = startPos.Offset; romOffset < endPos.Offset; romOffset++)
            {
                data[romOffset] = blockSet[blockIndex++];
            }
        }

        /// <summary>
        /// Commits the local data to the ROM data
        /// </summary>
        private void SaveItemListToRom(Level saveLevel)
        {
            int itemAddress = LevelItemAddress(saveLevel.LevelNumber);

            // Save the player start position
            SaveItemDataAtOffset(itemAddress, saveLevel.PlayerStart, Level.playerStartOffset);

            // Save exit position
            SaveItemDataAtOffset(itemAddress, saveLevel.Exit.Position, Level.doorPositionOffset);

            // Save key position
            SaveItemDataAtOffset(itemAddress, saveLevel.Key.Position, Level.keyPositionOffset);

            // Save spawn points
            SaveItemDataAtOffset(itemAddress, saveLevel.SpawnPos1.Position, Level.spawn1Offset);
            SaveItemDataAtOffset(itemAddress, saveLevel.SpawnPos2.Position, Level.spawn2Offset);

            // Save all the item data.
            List<byte> itemBytes = saveLevel.GetItemListOutput();
            // We have the bytes we need to save the items
            // Save each one to the rom
            for (int i = 0; i < itemBytes.Count; i++)
            {
                SaveItemDataAtOffset(itemAddress, itemBytes[i], Level.itemSetOffset + i);
            }
        }

        private void SaveItemDataAtOffset(int LevelItemAddress, byte SaveData, int ItemOffset)
        {
            data[LevelItemAddress + ItemOffset] = SaveData;
        }

        /// <summary>
        /// This is an example of a data structure you might find in a ROM,
        /// where two bytes specify a screen coordinate.
        /// </summary>
        struct sampleDataStructure
        {
            //public byte x, y;
        }


        /// <summary>
        /// Returns the in-memory structured copy of the level.
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        public Level GetLevel(int Level)
        {
            return Levels[Level-1];
        }

        public List<Byte> GetItemData(int LevelNumber)
        {
            int levelItemAddress = LevelItemAddress(LevelNumber);
            List<Byte> itemsList = new List<Byte>();
            do
            {
                itemsList.Add(data[levelItemAddress++]);
            } while (!IsItemDelimiter(data[levelItemAddress]) && !IsItemZodiac(data[levelItemAddress]));
            if ((data[levelItemAddress] & 0xF0) == zodiacItem)
            {
                // Add the zodiac item and get ready to add the last byte
                itemsList.Add(data[levelItemAddress++]);
            }

            // Add last byte.  It is either the delimiter or the position of the zodiac
            itemsList.Add(data[levelItemAddress]);

            return itemsList;

        }

        public List<Byte> GetEnemyData(int LevelNumber)
        {
            int levelEnemyAddress = LevelEnemyAddress(LevelNumber);
            List<Byte> enemyList = new List<Byte>();
            do
            {
                enemyList.Add(data[levelEnemyAddress++]);
            } while (!IsEnemyDelimiter(data[levelEnemyAddress]));
            
            // Add last byte.  It is either the delimiter or the position of the zodiac
            enemyList.Add(data[levelEnemyAddress]);

            return enemyList;
        }


        public static bool IsItemZodiac(byte p)
        {
            return (p & 0xF0) == zodiacItem;
        }

        public static bool IsEnemyDelimiter(byte p)
        {
            return (p & 0xFF) == enemyDelimiter;
        }

        public static bool IsItemRepeater(byte p)
        {
            return (p & itemRepeat) == itemRepeat;
        }

        public static bool IsItemDelimiter(byte p)
        {
            return (p & 0xF0) == itemDelimiter;
        }


        private pHRom GetAddressByPointers(pHRom HiByte, pHRom LowByte, int LevelNumber)
        {
            byte hi = data[HiByte.Offset + LevelNumber - 1];
            byte lo = data[LowByte.Offset + LevelNumber - 1];
            pHRom pointerOffset = new pHRom((hi << 8) + lo);

            return new pHRom(pointerOffset.Offset - 0x8000 + 0x10);
        }

        /// <summary>
        /// Stores the names of items by in-game index
        /// </summary>
        private static Dictionary<int, String> ItemNameLookup;
        public static String GetItemName(int itemIndex)
        {
            int maskedIndex = (itemIndex & 0x3F);
            return ItemNameLookup.ContainsKey(maskedIndex) ? ItemNameLookup[maskedIndex] : null;
        }
        private void PopulateItemNames()
        {
            if (ItemNameLookup != null)
                return;

            ItemNameLookup = new Dictionary<int, string>();
            ItemNameLookup.Add(ItemIndexConstants.BellIndex, "Bell");
        }

        /// <summary>
        /// Stores the names of monsters by in-game index
        /// </summary>
        private static Dictionary<int, String> EnemyNameLookup;
        public static String GetEnemyName(int enemyIndex)
        {
            return EnemyNameLookup.ContainsKey(enemyIndex) ? EnemyNameLookup[enemyIndex] : null;
        }
        private void PopulateEnemyNames()
        {
            if (EnemyNameLookup != null)
                return;

            EnemyNameLookup = new Dictionary<int, string>();
        }
    }
}
