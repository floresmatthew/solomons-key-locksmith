using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romulus.Nes;
using Locksmith.Data;

namespace Locksmith
{
    public enum GameSprites
    {
        Dana
    }

    public static class SolKeySprites
    {
        private static Dictionary<GameSprites, SpriteDef> Sprites =
            new Dictionary<GameSprites, SpriteDef>();

        static SolKeySprites()
        {

        }

        #region Sprite Tile Definitions
        private static byte[] DanaTiles = { 0x10, 0x12, 0x01, 0x03 };
        #endregion

        #region Sprite Methods
        public static SpriteDef Dana(byte xpos, byte ypos) {
            return GetSprite(DanaTiles, xpos, ypos);
        }
        #endregion

        #region Sprite Helper Methods
        /// <summary>
        /// Stores one tile as a sprite
        /// </summary>
        /// <param name="SpriteType"></param>
        /// <param name="TileOffset"></param>
        public static void StoreSpriteDef(GameSprites SpriteType, SpriteTile[] SpriteTiles)
        {
            Sprites.Add(SpriteType, new SpriteDef(SpriteTiles));
        }

        public static SpriteTile TilesToSprites(byte TileIndex, byte xpos, byte ypos)
        {
            SpriteTile sprite = new SpriteTile(TileIndex, xpos, ypos, Blitter.FlipFlags.None);
            return sprite;
        }

        private static SpriteDef GetSprite(byte[] Indexes, byte xpos, byte ypos)
        {
            return new SpriteDef(new SpriteTile[] 
                {
                    new SpriteTile((byte)Indexes[0], xpos, ypos, Blitter.FlipFlags.None),
                    new SpriteTile((byte)Indexes[1], (byte)(xpos + 8), ypos, Blitter.FlipFlags.None),
                    new SpriteTile((byte)Indexes[2], xpos, (byte)(ypos + 8), Blitter.FlipFlags.None),
                    new SpriteTile((byte)Indexes[3], (byte)(xpos + 8), (byte)(ypos + 8), Blitter.FlipFlags.None)
                });
        }
        #endregion

    }

    // only need a few printed
    public enum AlphaNumericTiles
    {
        One, Zero, S, C, O, R, E, L, I, F, A, Y
    }

    public enum GameTiles
    {
        None,
        Door,
        Key,
        SolidBlock,
        MagicBlock,
        MagicSeal1,
        MagicSeal2,
        MagicSeal3,
        MagicSeal4,
        SolomonSeal,
        BackgroundTile,
        Bell,
        SmallFlameJar,
        LargeFlameJar,
        SmallJewel,
        LargeJewel,
        BlueDiamond,
        GoldDiamond,
        Scroll,
        SpawnShield,
        SpawnBat,
        Blank,
        FakeDoor,
        FakeOpenDoor,
        FakeOpenDoor2,
        FakeOpenDoor3,
        FakeBrokenBlock,
        HalfLifeJar,
        FullLifeJar,
        HalfHourGlass,
        FullHourGlass,
        YellowJewel_5k,
        Star_10k,
        Star_20k,
        YellowJewel_50k,
        Origami,
        DemonHead,
        Sphinx,
        Helmet,
        Lamp,
        OneUpJar,
        KillJar,
        BlueKey,
        BlueFireExtend,
        Page,
        WarpFlag,
        GraySphere_100,
        GraySphere_200,
        GoldSphere_1k,
        GoldSphere_2k,

        Border_Right_Top,
        Border_Right_Mid,
        Border_Right_Bottom,
        Border_Left_Top,
        Border_Left_Mid,
        Border_Left_Bottom,
        Border_Top_Middle,

        Zodiac_1,
        Zodiac_2,
        Zodiac_3,
        Zodiac_4,

        TopScrollLeft,
        TopScrollLeftTop,
        TopScrollMid,
        TopScrollMidTopBreak,
        TopScrollRight,
        TopScrollRightTop,
        Dana
    }

    public static class SolKeyTiles
    {
        private class ZodiacTileDef
        {
            public ZodiacTileDef(int ppuRow)
            {
                byte bPPURow = (byte)ppuRow;
                byte[] Row1 = { 0x28, 0x01, 0x00, 0x05, 0x04, 0x29 };
                byte[] Row2 = { 0x2A, 0x03, 0x02, 0x07, 0x06, 0x2B };
                byte[] Row3 = { 0x28, 0x09, 0x08, 0x0D, 0x0C, 0x29 };
                byte[] Row4 = { 0x2A, 0x0B, 0x0A, 0x0F, 0x0E, 0x2B };

                Rows = new byte[4][];
                Rows[0] = Row1;
                Rows[1] = Row2;
                Rows[2] = Row3;
                Rows[3] = Row4;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        Rows[i][j] += bPPURow;
                    }
                }

            }

            byte[][] Rows;

            byte[] row1;
            byte[] row2;
            byte[] row3;
            byte[] row4;

            public TileEntry[,] ToTileEntry()
            {
                return ToTileEntry(0x03);
            }

            public TileEntry[,] ToTileEntry(int paletteIndex)
            {
                TileEntry[,] tEntry = new TileEntry[4, 6];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        tEntry[i, j] = new TileEntry((byte)(Rows[i][j]), (byte)paletteIndex);
                    }
                }

                return tEntry;
            }
        }

        private static void AddItemDefinition(GameTiles TileType, int Index, int PatternOffset, byte Palette)
        {
            Tiles.Add(TileType, GetTile_2x2((byte)PatternOffset, (byte)Palette));
            TileIndex.Add((byte)Index, TileType);
        }

        static SolKeyTiles()
        {
            // Load alpha numerics
            AlphaTiles.Add(AlphaNumericTiles.One, GetSingleTile(0x01));
            AlphaTiles.Add(AlphaNumericTiles.Zero, GetSingleTile(0x00));
            AlphaTiles.Add(AlphaNumericTiles.A, GetSingleTile(0x0A));
            AlphaTiles.Add(AlphaNumericTiles.C, GetSingleTile(0x0C));
            AlphaTiles.Add(AlphaNumericTiles.E, GetSingleTile(0x0E));
            AlphaTiles.Add(AlphaNumericTiles.F, GetSingleTile(0x0F));
            AlphaTiles.Add(AlphaNumericTiles.I, GetSingleTile(0x12));
            AlphaTiles.Add(AlphaNumericTiles.L, GetSingleTile(0x15));
            AlphaTiles.Add(AlphaNumericTiles.O, GetSingleTile(0x18));
            AlphaTiles.Add(AlphaNumericTiles.R, GetSingleTile(0x1B));
            AlphaTiles.Add(AlphaNumericTiles.S, GetSingleTile(0x1C));
            AlphaTiles.Add(AlphaNumericTiles.Y, GetSingleTile(0x22));

            // Zodiac tiles.  Totally disorganized.
            /*
             * Zodiac 1
             * 0xC1 0xC0 0xC5 0xC4
             * 0xC3 0xC2 0xC7 0xC6
             * 0xC9 0xC8 0xCD 0xCC
             * 0xCB 0xCA 0xCF 0xCE
             * 
             * other zodiac symbols should follow suit
             */

            //ZodiacTileDef zod1 = new ZodiacTileDef(
            //    new byte[] {0xC1, 0xC0, 0xC5, 0xC4},
            //    new byte[] {0xC3, 0xC2, 0xC7, 0xC6},
            //    new byte[] {0xC9, 0xC8, 0xCD, 0xCC},
            //    new byte[] {0xCB, 0xCA, 0xCF, 0xCE}
            //    );

            Tiles.Add(GameTiles.Zodiac_1, new ZodiacTileDef(0xC0).ToTileEntry());
            TileIndex.Add(0xF0, GameTiles.Zodiac_1);
            TileIndex.Add(0xF4, GameTiles.Zodiac_1);
            TileIndex.Add(0xF8, GameTiles.Zodiac_1);

            Tiles.Add(GameTiles.Zodiac_2, new ZodiacTileDef(0xD0).ToTileEntry());
            TileIndex.Add(0xF1, GameTiles.Zodiac_2);
            TileIndex.Add(0xF5, GameTiles.Zodiac_2);
            TileIndex.Add(0xF9, GameTiles.Zodiac_2);

            Tiles.Add(GameTiles.Zodiac_3, new ZodiacTileDef(0xE0).ToTileEntry());
            TileIndex.Add(0xF2, GameTiles.Zodiac_3);
            TileIndex.Add(0xF6, GameTiles.Zodiac_3);
            TileIndex.Add(0xFA, GameTiles.Zodiac_3);

            Tiles.Add(GameTiles.Zodiac_4, new ZodiacTileDef(0xF0).ToTileEntry());
            TileIndex.Add(0xF3, GameTiles.Zodiac_4);
            TileIndex.Add(0xF7, GameTiles.Zodiac_4);
            TileIndex.Add(0xFB, GameTiles.Zodiac_4);

            // Load the tiles
            Tiles.Add(GameTiles.BackgroundTile, NewBackground());
            Tiles.Add(GameTiles.Blank, GetTile(0x24));
            Tiles.Add(GameTiles.Door, GetTile_2x2((byte)0x40, (byte)0x01));
            Tiles.Add(GameTiles.MagicBlock, GetTile_2x2((byte)0x90, (byte)0x01));

            // Assign the indexes
            AddItemDefinition(GameTiles.FakeBrokenBlock, 0x01, 0x94, 0x01);
            AddItemDefinition(GameTiles.FakeDoor, 0x02, 0x40, 0x01);
            AddItemDefinition(GameTiles.SolidBlock, 0x03, 0x84, 0x00);
            AddItemDefinition(GameTiles.SpawnBat, 0x04, 0x3C, 0x00);
            AddItemDefinition(GameTiles.SpawnShield, 0x05, 0x58, 0x01);
            
            // Special for key
            Tiles.Add(GameTiles.Key, GetTile_2x2((byte)0x8D, (byte)0x01, true));
            TileIndex.Add(0x06, GameTiles.Key); // NEVER EVER USE THIS BECAUSE IT SCREWS UP THE GAME
            // IT'S ONLY HERE SO NOBODY SAYS 'Hey, wonder why Key isn't added.'
            
            AddItemDefinition(GameTiles.FakeOpenDoor, 0x07, 0x44, 0x01);
            AddItemDefinition(GameTiles.BlueDiamond, 0x08, 0x70, 0x02);
            // 0x09 - 0xb are just states of the blue diamond.  should never appear in the wild, but add them if you want.
            
            AddItemDefinition(GameTiles.GoldDiamond, 0x0c, 0x70, 0x01);
            // 0x0d - 0xf are just states of the gold diamond.  should never appear in the wild, but add them if you want.
            // 0x10 blank

            //AddItemDefinition(GameTiles.HalfLifeJar, 0x11, 0x40, 0x01);
            AddItemDefinition(GameTiles.FullLifeJar, 0x12, 0x4C, 0x00);
            AddItemDefinition(GameTiles.FullHourGlass, 0x13, 0x78, 0x02);
            AddItemDefinition(GameTiles.HalfHourGlass, 0x14, 0x7C, 0x01);
            //AddItemDefinition(GameTiles.SmallFlameJar, 0x15, 0x64, 0x02);
            TileIndex.Add(0x15, GameTiles.SmallFlameJar);
            Tiles.Add(GameTiles.SmallFlameJar, GetMultiTile(0x60, 0x65, 0x66, 0x67, 0x02));
            //AddItemDefinition(GameTiles.LargeFlameJar, 0x16, 0x64, 0x01);
            TileIndex.Add(0x16, GameTiles.LargeFlameJar);
            Tiles.Add(GameTiles.LargeFlameJar, GetMultiTile(0x60, 0x65, 0x66, 0x67, 0x01));
            // Special for Scroll
            Tiles.Add(GameTiles.Scroll, GetTile_2x2(0x99, 0x01, true));
            TileIndex.Add(0x17, GameTiles.Scroll);
            
            AddItemDefinition(GameTiles.Bell, ItemIndexConstants.BellIndex, 0x9C, 0x01);
            AddItemDefinition(GameTiles.KillJar, ItemIndexConstants.KillJar, 0x4C, 0x01);
            AddItemDefinition(GameTiles.BlueKey, ItemIndexConstants.BlueKey, 0x8C, 0x02);
            AddItemDefinition(GameTiles.BlueFireExtend, ItemIndexConstants.BlueFireExtend, 0x6C, 0x02);
            AddItemDefinition(GameTiles.MagicSeal1, ItemIndexConstants.MagicSeal1, 0x50, 0x01);
            AddItemDefinition(GameTiles.MagicSeal2, ItemIndexConstants.MagicSeal2, 0x54, 0x01);
            AddItemDefinition(GameTiles.MagicSeal3, ItemIndexConstants.MagicSeal3, 0x5C, 0x01);
            AddItemDefinition(GameTiles.MagicSeal4, ItemIndexConstants.MagicSeal4, 0x74, 0x01);
            
            AddItemDefinition(GameTiles.SolomonSeal, 0x20, 0x68, 0x00);
            AddItemDefinition(GameTiles.Page, 0x21, 0x88, 0x03); // Page of Space/Time, depends on tile set
            AddItemDefinition(GameTiles.WarpFlag, 0x22, 0xAC, 0x01);
            AddItemDefinition(GameTiles.FakeOpenDoor2, 0x23, 0x44, 0x00);  // BAD if this is touched.  DO NOT USE
            AddItemDefinition(GameTiles.FakeOpenDoor3, 0x24, 0x44, 0x00);  // BAD if this is touched.  DO NOT USE
            AddItemDefinition(GameTiles.GraySphere_100, 0x25, 0x34, 0x00);

            // Special for GoldSphere_200
            TileIndex.Add(0x26, GameTiles.GraySphere_200);
            Tiles.Add(GameTiles.GraySphere_200, GetMultiTile(0x34, 0x80, 0x36, 0x82, 0x00));
            AddItemDefinition(GameTiles.SmallJewel, 0x27, 0x30, 0x02);
            AddItemDefinition(GameTiles.GoldSphere_1k, 0x28, 0x34, 0x03);
            
            // Special for GoldSphere_2k
            TileIndex.Add(0x29, GameTiles.GoldSphere_2k);
            Tiles.Add(GameTiles.GoldSphere_2k, GetMultiTile(0x34, 0x80, 0x36, 0x82, 0x03));

            AddItemDefinition(GameTiles.YellowJewel_5k, 0x2a, 0x30, 0x01);
            AddItemDefinition(GameTiles.Star_10k, 0x2b, 0x38, 0x03);

            // Special for Star_20k
            TileIndex.Add(0x2c, GameTiles.Star_20k);
            Tiles.Add(GameTiles.Star_20k, GetMultiTile(0x38, 0x81, 0x3A, 0x83, 0x03));

            AddItemDefinition(GameTiles.YellowJewel_50k, 0x2d, 0x30, 0x03);
            
            // Special for Origami
            TileIndex.Add(0x2e, GameTiles.Origami);
            Tiles.Add(GameTiles.Origami, GetMultiTile(0xA8, 0xA9, 0x49, 0x4B, 0x03)); // 100k
            // Special for DemonHead
            TileIndex.Add(0x2f, GameTiles.DemonHead);
            Tiles.Add(GameTiles.DemonHead, GetMultiTile(0x20, 0x21, 0x1A, 0x23, 0x01)); // 200k
            
            // Special for Sphinx
            TileIndex.Add(0x30, GameTiles.Sphinx);
            Tiles.Add(GameTiles.Sphinx, GetMultiTile(0x10, 0x13, 0x19, 0x19, 0x01)); // 500k
            AddItemDefinition(GameTiles.Helmet, 0x31, 0x88, 0x01); // 1M
            
            // Special for DemonHead
            TileIndex.Add(0x32, GameTiles.Lamp);
            Tiles.Add(GameTiles.Lamp, GetMultiTile(0x14, 0x1D, 0x1E, 0x1F, 0x03)); // 500k
            AddItemDefinition(GameTiles.OneUpJar, 0x33, 0x60, 0x01);
            // 34-36 are also 1-ups.

            // Left Border
            TileIndex.Add(0xB0, GameTiles.Border_Right_Top);
            Tiles.Add(GameTiles.Border_Right_Top, GetTile(0xA4));
            TileIndex.Add(0xB1, GameTiles.Border_Right_Mid);
            Tiles.Add(GameTiles.Border_Right_Mid, GetTile(0xA6));
            TileIndex.Add(0xB2, GameTiles.Border_Right_Bottom);
            Tiles.Add(GameTiles.Border_Right_Bottom, GetTile(0xA3));

            // Right Border
            TileIndex.Add(0xB3, GameTiles.Border_Left_Top);
            Tiles.Add(GameTiles.Border_Left_Top, GetTile(0x84));
            TileIndex.Add(0xB4, GameTiles.Border_Left_Mid);
            Tiles.Add(GameTiles.Border_Left_Mid, GetTile(0xA2));
            TileIndex.Add(0xB5, GameTiles.Border_Left_Bottom);
            Tiles.Add(GameTiles.Border_Left_Bottom, GetTile(0xAA));
            TileIndex.Add(0xB6, GameTiles.Border_Top_Middle);
            Tiles.Add(GameTiles.Border_Top_Middle, GetTile(0xA1));

            // Scroll in border
            TileIndex.Add(0xC0, GameTiles.TopScrollLeft);
            Tiles.Add(GameTiles.TopScrollLeft, GetTile(0xA7));
            TileIndex.Add(0xC1, GameTiles.TopScrollMid);
            Tiles.Add(GameTiles.TopScrollMid, GetTile(0xB6));
            TileIndex.Add(0xC2, GameTiles.TopScrollRight);
            Tiles.Add(GameTiles.TopScrollRight, GetTile(0xB7));
            TileIndex.Add(0xC4, GameTiles.TopScrollMidTopBreak);
            Tiles.Add(GameTiles.TopScrollMidTopBreak, GetTile(0xB0));
            TileIndex.Add(0xC5, GameTiles.TopScrollRightTop);
            Tiles.Add(GameTiles.TopScrollRightTop, GetTile(0xB4));
            TileIndex.Add(0xC6, GameTiles.TopScrollLeftTop);
            Tiles.Add(GameTiles.TopScrollLeftTop, GetTile(0xA5));
        }

        public static TileEntry[,] TopScroll
        {
            get
            {
                TileEntry[,] tEntry = new TileEntry[5, 2];
                tEntry[0, 0] = Tiles[GameTiles.TopScrollLeftTop][0, 0];
                tEntry[0, 1] = Tiles[GameTiles.TopScrollLeft][0, 0];
                tEntry[1, 0] = Tiles[GameTiles.TopScrollRightTop][0, 0];
                tEntry[1, 1] = Tiles[GameTiles.TopScrollMid][0, 0];

                tEntry[2, 0] = Tiles[GameTiles.TopScrollRightTop][0, 0];
                tEntry[2, 1] = Tiles[GameTiles.TopScrollMid][0, 0];
                tEntry[3, 0] = Tiles[GameTiles.TopScrollRightTop][0, 0];
                tEntry[3, 1] = Tiles[GameTiles.TopScrollMid][0, 0];

                tEntry[4, 0] = Tiles[GameTiles.TopScrollRightTop][0, 0];
                tEntry[4, 1] = Tiles[GameTiles.TopScrollRight][0, 0];
                return tEntry;
            }
        }

        public static TileEntry[,] BottomBorder
        {
            get
            {
                TileEntry[,] tEntry = new TileEntry[1, 4];
                tEntry[0, 0] = Tiles[GameTiles.Border_Right_Top][0, 0];
                tEntry[0, 1] = Tiles[GameTiles.Border_Left_Top][0, 0];
                tEntry[0, 2] = Tiles[GameTiles.Border_Top_Middle][0, 0];
                tEntry[0, 3] = Tiles[GameTiles.Border_Top_Middle][0, 0];
                return tEntry;
            }
        }

        public static TileEntry[,] LeftBorder
        {
            get
            {
                TileEntry[,] tEntry = new TileEntry[1, 4];
                tEntry[0, 0] = new TileEntry(0xA4, 0);
                tEntry[0, 1] = new TileEntry(0xA6, 0);
                tEntry[0, 2] = new TileEntry(0xA6, 0);
                tEntry[0, 3] = new TileEntry(0xA3, 0);
                return tEntry;
            }
        }

        public static TileEntry[,] RightBorder
        {
            get
            {
                TileEntry[,] tEntry = new TileEntry[1, 4];
                tEntry[0, 0] = new TileEntry(0x84, 0);
                tEntry[0, 1] = new TileEntry(0xA2, 0);
                tEntry[0, 2] = new TileEntry(0xA2, 0);
                tEntry[0, 3] = new TileEntry(0xAA, 0);
                return tEntry;
            }
        }


        private static Dictionary<GameTiles, TileEntry[,]> Tiles =
            new Dictionary<GameTiles, TileEntry[,]>();

        private static Dictionary<AlphaNumericTiles, TileEntry> AlphaTiles =
            new Dictionary<AlphaNumericTiles, TileEntry>();

        private static Dictionary<byte, GameTiles> TileIndex =
            new Dictionary<byte, GameTiles>();

        private static TileEntry[,] NewBackground()
        {
            byte Offset = 0x2C;
            byte TilePalette = 0x0;
            TileEntry[,] tEntry = GetTile_2x2(Offset, TilePalette);
            return tEntry;
        }

        private static TileEntry[,] GetMultiTile(byte TileOffset1, byte TileOffset2, byte TileOffset3, byte TileOffset4, byte TilePalette)
        {
            TileEntry[,] tEntry = new TileEntry[2, 2];
            tEntry[0, 0] = new TileEntry(TileOffset1, TilePalette);
            tEntry[0, 1] = new TileEntry(TileOffset2, TilePalette);
            tEntry[1, 0] = new TileEntry(TileOffset3, TilePalette);
            tEntry[1, 1] = new TileEntry(TileOffset4, TilePalette);
            return tEntry;
        }

        private static TileEntry[,] GetTile_2x2(byte Offset, byte TilePalette, bool BlankCorner)
        {
            TileEntry[,] tEntry = new TileEntry[2, 2];
            tEntry[0, 0] = new TileEntry(0x2C, TilePalette);
            tEntry[0, 1] = new TileEntry(Offset, TilePalette);
            tEntry[1, 0] = new TileEntry((byte)(Offset + 0x1), TilePalette);
            tEntry[1, 1] = new TileEntry((byte)(Offset + 0x2), TilePalette);
            return tEntry;
        }

        private static TileEntry[,] GetTile_2x2(byte Offset, byte TilePalette)
        {
            TileEntry[,] tEntry = new TileEntry[2, 2];
            tEntry[0, 0] = new TileEntry(Offset, TilePalette);
            tEntry[0, 1] = new TileEntry((byte)(Offset + 0x1), TilePalette);
            tEntry[1, 0] = new TileEntry((byte)(Offset + 0x2), TilePalette);
            tEntry[1, 1] = new TileEntry((byte)(Offset + 0x3), TilePalette);

            return tEntry;
        }
        public static TileEntry Zero { get { return AlphaTiles[AlphaNumericTiles.Zero]; } }
        public static TileEntry One { get { return AlphaTiles[AlphaNumericTiles.One]; } }
        public static TileEntry AlphaTile(string alpha) { return AlphaTiles.Single(i => i.Key.ToString() == ( alpha == "1" ? "One" : alpha == "0" ? "Zero" : alpha)).Value; }

        public static TileEntry[,] Door
        {
            get
            {
                return Tiles[GameTiles.Door];
            }
        }
        public static TileEntry[,] Key
        {
            get { return Tiles[GameTiles.Key]; }
        }
        public static TileEntry[,] MagicSeal_1
        {
            get
            {
                return Tiles[GameTiles.MagicSeal1];
            }
        }
        public static TileEntry[,] EvilMirror
        {
            get
            {
                return GetTile_2x2((byte)0x58, (byte)0x01);
            }
        }
        public static TileEntry[,] MagicBlock
        {
            get
            {
                return Tiles[GameTiles.MagicBlock];
            }
        }
        public static TileEntry[,] SolidBlock
        {
            get
            {
                return Tiles[GameTiles.SolidBlock];
            }
        }
        public static TileEntry[,] SpawnShield
        {
            get
            { return Tiles[GameTiles.SpawnShield]; }
        }
        public static TileEntry[,] BackgroundTile
        {
            get
            {
                return Tiles[GameTiles.BackgroundTile];
            }
        }

        public static TileEntry[,] BlankTile
        { get { return Tiles[GameTiles.Blank]; } }

        private static TileEntry GetSingleTile(byte Offset)
        {
            return new TileEntry(Offset, 0);
        }

        private static TileEntry[,] GetTile(byte Offset)
        {
            TileEntry[,] tEntry = new TileEntry[1, 1];
            tEntry[0, 0] = new TileEntry(Offset, 0);

            return tEntry;
        }
        public static TileEntry[,] GetTileByIndex(byte index)
        {
            if (TileIndex.ContainsKey(index))
                return Tiles[TileIndex[index]];
            else return null;
        }
    }
}
