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
    public class BottomBorder:ScreenControl
    {
        internal new SolRom Rom { get { return base.Rom as SolRom; } set { base.Rom = value; } }

        // This is where we prepare to render.
        protected override void BeforeRender() {
            base.BeforeRender();
            Patterns = Rom.bgPatterns[0].PatternImage;
        }

        protected override void LoadBgPalette(ReturnEventArgs<CompositePalette> e)
        {
            base.LoadBgPalette(e);

            e.ReturnValue = palette;
        }

        protected override void ConstructNametable() {
            base.ConstructNametable();

            TileEntry[,] borderTiles = SolKeyTiles.BottomBorder;
            for (int i = 0; i < 32; )
            {
                // Load tile at coordinates (0, i)
                for (int j = 0; j < 4; j++)
                    NameTable[i++, 0] = borderTiles[0, j];
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

        static BottomBorder()
        {
            palette = new CompositePalette(paletteData, 0);
            spritePalette = new CompositePalette(spritePaletteData, 0);
        }
        #endregion
    }
}
