using System.Linq;
using System.Collections.Generic;
using Romulus;
using Romulus.Nes;
using Locksmith.Data;
using System.Windows.Forms;

namespace Locksmith.Forms
{
    /// <summary>
    /// Demonstrates 
    /// </summary>
    public class TopBorder:ScreenControl
    {
        private const int textRow = 1;
        private const int numRow = 2;
        public BorderSide Side { get; set; }
        internal new SolRom Rom { get { return base.Rom as SolRom; } set { base.Rom = value; } }

        // This is where we prepare to render.
        protected override void BeforeRender() {
            base.BeforeRender();
            Patterns = Rom.bgPatterns[0].PatternImage;
        }

        protected override void LoadBgPalette(ReturnEventArgs<CompositePalette> e) {
            base.LoadBgPalette(e);

            e.ReturnValue = palette;
        }
        protected override void LoadSpritePalette(ReturnEventArgs<CompositePalette> e) {
            base.LoadSpritePalette(e);

            e.ReturnValue = spritePalette;
            // This is where we would return the sprite palette
        }
        
        protected override void ConstructNametable() {
            base.ConstructNametable();

            /*
             * Top area shows
             * SCORE  LIFE FAIRY
             *     0 10000   0   (scroll)
             */

            // Add blank tiles all over
            for (int i = 0; i < base.CellSize.Height / 8; i++)
            {
                for (int j = 0; j < base.CellSize.Width / 8; j++)
                {
                    NameTable[i, j] = SolKeyTiles.BlankTile[0,0];
                }
            }

            AddTextToNametable("SCORE", 4, textRow);
            AddTextToNametable("0", 8, numRow);
            AddTextToNametable("LIFE", 11, textRow);
            AddTextToNametable("10000", 10, numRow);
            AddTextToNametable("FAIRY", 16, textRow);
            AddTextToNametable("0", 18, numRow);

            // Add scroll image

            TileEntry[,] scrollTiles = SolKeyTiles.TopScroll;
            int scrollXpos = 21;
            for (int i = 0; i < 5; i++)
            {
                int scrollYpos = 1;
                // Load tile at coordinates (0, i)
                for (int j = 0; j < 2; j++)
                {
                    NameTable[scrollXpos, scrollYpos] = scrollTiles[i, j];
                    scrollYpos++;
                }
                scrollXpos++;
            }

        }

        private void AddTextToNametable(string text, int startColumn, int row)
        {
            System.CharEnumerator charEnum = text.GetEnumerator();
            charEnum.Reset();
            int offset = 0;
            while (charEnum.MoveNext())
            {
                NameTable[startColumn + offset++, row] = SolKeyTiles.AlphaTile(charEnum.Current.ToString());
            }
        }

        #region Static
        // This data would normally come from the rom

        static byte[] spritePaletteData = new byte[] {
            0x0, 0x26, 0x29, 0x30, 
            0x0, 0x30, 0x16, 0x27, 
            0x0, 0x16, 0x10, 0x30, 
            0x0, 0x2c, 0x26, 0x38
        };

        static byte[] paletteData = new byte[] { 
            0xF, 0x7, 0x10, 0x30, 
            0xF, 0x7, 0x27, 0x30, 
            0xF, 0x7, 0x2c, 0x30, 
            0xF, 0x7, 0x27, 0x38
        };
        static CompositePalette palette;
        static CompositePalette spritePalette;

        static TopBorder() {
            palette = new CompositePalette(paletteData, 0);
            spritePalette = new CompositePalette(spritePaletteData, 0);
        }
        #endregion
    }
}
