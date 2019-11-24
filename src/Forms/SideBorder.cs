using System.Collections.Generic;
using Romulus;
using Romulus.Nes;
using Locksmith.Data;
using System.Windows.Forms;

namespace Locksmith.Forms
{
    public enum BorderSide
    {
        Left, Right
    }
    /// <summary>
    /// Demonstrates 
    /// </summary>
    public class SideBorder:ScreenControl
    {
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
             * Load border tiles like so
             * 1
             * 2
             * 3
             * 4
             * 1
             * 2
             * ...
             */
            TileEntry[,] borderTiles = (Side == BorderSide.Left ? SolKeyTiles.LeftBorder : SolKeyTiles.RightBorder);
            for (int i = 0; i < 24; )
            {
                // Load tile at coordinates (0, i)
                for (int j = 0; j < 4; j++)
                    NameTable[0, i++] = borderTiles[0,j];
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

        static SideBorder() {
            palette = new CompositePalette(paletteData, 0);
            spritePalette = new CompositePalette(spritePaletteData, 0);
        }
        #endregion
    }
}
