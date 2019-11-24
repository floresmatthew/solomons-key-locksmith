using System;
using System.Linq;
using System.Collections.Generic;
using Romulus;
using Romulus.Nes;
using Locksmith.Data;
using System.Windows.Forms;

namespace Locksmith.Forms
{
    /// <summary>
    /// Main game screen for Solomon's Key
    /// </summary>
    public class SolScreenControl : ScreenControl
    {
        public ActionMode Mode;
        public Point StartMousePoint;
        public SolKeyLevelElement ActiveElement;

        // Helper to expose Rom as our specific ROM type.
        internal new SolRom Rom { get { return base.Rom as SolRom; } set { base.Rom = value; } }

        // What level to render;
        public int LevelNum { get; set; }
        private Level level;
        private List<SpriteDef> LevelSprites = new List<SpriteDef>();
        public GameTiles ActiveTile { get; set; }

        // Toggle layers
        public bool DisplaySolidBlocks { get; set; }
        public bool DisplayMagicBlocks { get; set; }
        public bool DisplayEnemies { get; set; }
        public bool DisplayItems { get; set; }

        public SolScreenControl()
        {
            DisplaySolidBlocks = true;
            DisplayMagicBlocks = true;
            DisplayEnemies = true;
            DisplayItems = true;
        }

        // This is where we prepare to render.
        protected override void BeforeRender()
        {
            base.BeforeRender();
            LevelSprites.Clear();

            if (Rom == null)
                return;
            // This is where you should set you bg patterns (and sprite patterns, if applicable)
            // Each level may have its own background pattern
            level = Rom.GetLevel(LevelNum);
            Patterns = level.BackgroundPatterns.PatternImage;
            SpritePatterns = Rom.spritePatterns.PatternImage;
        }

        protected override void LoadBgPalette(ReturnEventArgs<CompositePalette> e)
        {
            base.LoadBgPalette(e);

            e.ReturnValue = palette;
        }
        protected override void LoadSpritePalette(ReturnEventArgs<CompositePalette> e)
        {
            base.LoadSpritePalette(e);

            e.ReturnValue = spritePalette;
            // This is where we would return the sprite palette
        }

        protected override void ConstructNametable()
        {
            base.ConstructNametable();

            if (Rom == null)
                return;

            TileEntry[,] backgroundTiles = SolKeyTiles.BackgroundTile;
            // Draw all tiles for this quadrant (16x16 tiles)
            
            for (int i = 0; i < 30; i+=2)
            {
                for (int j = 0; j < 16; j++)
                {
                    AddTileBlockToNameTable(backgroundTiles, j, i);
                    //NameTable[i, j] = backgroundTiles[j%2,i%2];
                }
            }

            if (level.Constellation != null)
                AddSolKeyElementToNameTable(level.Constellation, false, 2);

            // Load magic blocks
            if (DisplayMagicBlocks)
                LoadBlocksInLevel(level, level.MagicBlocks, SolKeyTiles.MagicBlock);

            // Load solid blocks
            if (DisplaySolidBlocks)
                LoadBlocksInLevel(level, level.SolidBlocks, SolKeyTiles.SolidBlock);

            // Load exit
            AddSolKeyElementToNameTable(level.Exit);
            //AddTileBytePos(SolKeyTiles.Door, level.Exit);
            AddSolKeyElementToNameTable(level.Key);
            //AddTileBytePos(SolKeyTiles.Key, level.Key);

            // Load spawn points
            AddSolKeyElementToNameTable(level.SpawnPos1);
            AddSolKeyElementToNameTable(level.SpawnPos2);
            //AddTileBytePos(SolKeyTiles.SpawnShield, level.SpawnPos1);
            //AddTileBytePos(SolKeyTiles.SpawnShield, level.SpawnPos2);

            if (DisplayItems)
            {
                foreach (SolKeyLevelElement e in level.ItemsList)
                {
                    AddSolKeyElementToNameTable(e);
                }
            }

        }

        private void AddSolKeyElementToNameTable(SolKeyLevelElement e)
        {
            AddSolKeyElementToNameTable(e, true, -1);
        }

        private void AddSolKeyElementToNameTable(SolKeyLevelElement e, int paletteOverride)
        {
            AddSolKeyElementToNameTable(e, true, paletteOverride);
        }

        private void AddSolKeyElementToNameTable(SolKeyLevelElement e, bool maskIndex)
        {
            AddSolKeyElementToNameTable(e, maskIndex, -1);
        }

        private void AddSolKeyElementToNameTable(SolKeyLevelElement e, bool maskIndex, int paletteOverride)
        {
            // index & 0x3F because we don't care if the item is in a block or invisible
            // x1xx xxxx item is invisible
            // 1xxx xxxx item is in a block

            // Use a value of 0xFF to get all bytes if the first two bits matter.
            // Example, Zodiac can have an index of 0xF0, which actually turns into 0x30 if we apply the default mask

            int maskValue = maskIndex ? 0x3F : 0xFF;

            TileEntry[,] ItemTiles = SolKeyTiles.GetTileByIndex((byte)(e.ElementIndex & maskValue));
            if (ItemTiles != null)
            {
                if (paletteOverride > -1)
                    //ItemTiles[1,1].Palette = (byte)paletteOverride;
                    ;
                AddTileBytePos(ItemTiles, e.Position);
            }
        }

        private void AddTileBytePos(TileEntry[,] Tile, byte Position)
        {
            int xPos = Utility.GetXPosition(Position);
            int yPos = Utility.GetYPosition(Position);
            AddTileBlockToNameTable(Tile, xPos, yPos * 2);
        }

        private void LoadBlocksInLevel(Level level, byte[] BlockArray, TileEntry[,] BlockTile)
        {
            int yPos = 2;
            for (int i = 0; i < BlockArray.Length; i++)
            {
                // for the right half of the screen, we want to offset the xpos by 8 blocks
                int xPosOffset = 0;

                if (i % 2 != 0)
                {
                    xPosOffset = 8;
                }
                byte b = BlockArray[i];
                for (int xPos = 0; xPos < 16; xPos += 1)
                {
                    // Add block to table at the appropriate x,y
                    if (BlockAtXPos(b, xPos))
                        AddTileBlockToNameTable(BlockTile, xPos + xPosOffset, yPos);
                }
                if (i % 2 != 0)
                {
                    yPos += 2;
                }
            }
        }

        private void AddTileBlockToNameTable(TileEntry[,] tiles, int xPos, int yPos)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    NameTable[(xPos * 2 + x), (yPos + y)] = tiles[y, x];
                }
            }
        }

        private bool BlockAtXPos(byte data, int xPos)
        {
            return ((data & (0x80 >> xPos)) >> (7 - xPos) == 1);
        }

        protected override void ConstructSpriteList(EventArgs<NameTableRenderer> e)
        {
            // This is where you would construct your sprite list
            // (You can safely ignore e and use the this.NameTable property)
            base.ConstructSpriteList(e);

            if (Rom == null)
                return;

            // Load each sprite (stored separately from tiles)
            // Load player
            AddPlayerSprite(level.PlayerStart);

            foreach (SpriteDef def in LevelSprites)
            {
                foreach (SpriteTile tile in def.Data)
                {
                    e.Value.Sprites.Add(tile);
                }
            }
        }

        private void AddPlayerSprite(byte position)
        {
            byte gridX = (byte)(Utility.GetXPosition(position) * 16);
            byte gridY = (byte)(Utility.GetYPosition(position) * 16);
            LevelSprites.Add(SolKeySprites.Dana(gridX, gridY));
        }

        public void ShowNextLevel()
        {
            ShowLevel(LevelNum + 1);
        }

        public void ShowPrevLevel()
        {
            ShowLevel(LevelNum - 1);
        }

        public void ShowLevel(int levelNumber)
        {
            if (levelNumber >= SolRom.MAX_LEVEL)
                LevelNum = SolRom.MAX_LEVEL;
            else if (levelNumber <= SolRom.MIN_LEVEL)
                LevelNum = SolRom.MIN_LEVEL;
            else
                LevelNum = levelNumber;

            RenderScreen();
        }

        private bool NextLevel()
        {
            if (LevelNum < SolRom.MAX_LEVEL)
            {
                LevelNum++;
                return true;
            }
            return false;
        }

        private bool PrevLevel()
        {
            if (LevelNum > SolRom.MIN_LEVEL)
            {
                LevelNum--;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Translates mouse click coordinates to in-game coordinates
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private byte GetGameCoordinates(MouseEventArgs e)
        {
            return GetGameCoordinates(e.X, e.Y);
        }

        /// <summary>
        /// Translates a mouse drag to in-game coordinates
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private byte GetGameCoordinates(DragEventArgs e)
        {
            return GetGameCoordinates(e.X, e.Y);
        }

        private byte GetGameCoordinates(int x, int y)
        {
            int gridX = (byte)(x / 16);
            int gridY = (byte)(y / 16);
            return (byte)(((gridY << 4) & 0xF0) + (gridX & 0x0F));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // if rom is not loaded, exit
            if (Rom == null)
                return;

            byte mousePos = GetGameCoordinates(e);

            if (e.Button == MouseButtons.Right)
                Mode = ActionMode.GetInfo;

            if (Mode == ActionMode.DragAndDrop)
            {
                SolKeyLevelElement element = GetElementAtTile(mousePos);
                if (element != null)
                {
                    ActiveElement = element;
                    StartMousePoint = Utility.GetPoint(mousePos);
                    ActiveTile = element.Tile;
                }
            }
            else
            {
                if (Mode == ActionMode.GetInfo)
                {
                    if (e.Button == MouseButtons.Right)
                        GetTileInformation(mousePos);
                }
                else
                    EditScreenWithActiveItem(mousePos);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // We're probably dragging an item or some other element.
            // Make sure we're in DragAndDrop mode
            if (Mode != ActionMode.DragAndDrop || ActiveElement == null)
                return;

            // We've moved, so change the position of our active element;
            byte newPosition = GetGameCoordinates(e);
            if (ActiveElement.Position != newPosition)
            {
                ActiveElement.Position = newPosition;
                RenderScreen();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            // If we're in drag and drop mode, we're dropping our element here.

            byte mousePos = GetGameCoordinates(e);
            if (Mode == ActionMode.DragAndDrop && ActiveElement != null)
            {
                // If we're dropping it on a position that has an item, remove the old one
                SolKeyLevelElement element = GetElementAtTile(mousePos);
                if (element != null)
                    level.RemoveItemAtPosition(element.Position);

                // Now remove the item at our starting position
                level.RemoveItemAtPosition(ActiveElement.Position);

                // Add the item to our new position
                level.AddItemAtPosition(ActiveElement, mousePos);

                ActiveElement = null;
                RenderScreen();
            }
        }

        private SolKeyLevelElement GetElementAtTile(byte mousePos)
        {
            SolKeyLevelElement element = null;
            /*
             * Pecking order:
             * Enemy
             * Dana
             * Item
             * Key
             * Door
             * Block
             */
            // Item
            element = level.ItemsList.FirstOrDefault(i => i.Position == mousePos);

            if (element != null) return element;

            if (level.SpawnPos1.Position == mousePos) element = level.SpawnPos1;
            else if (level.SpawnPos2.Position == mousePos) element = level.SpawnPos2;
            // Key
            else if (level.Key.Position == mousePos) element = level.Key;
            // Door
            else if (level.Exit.Position == mousePos) element = level.Exit;

            return element;
        }

        private void GetTileInformation(byte mousePos)
        {
            // If there is an item at this position, get the item info.
            OnItemInfo(new ItemInfoEventArgs(GetElementAtTile(mousePos)));
        }

        private void EditScreenWithActiveItem(byte newPos)
        {
            // Set level data
            switch (ActiveTile)
            {
                case GameTiles.Dana:
                    level.PlayerStart = newPos;
                    break;
                case GameTiles.Key:
                    level.Key.Position = newPos;
                    break;
                case GameTiles.Door:
                    level.Exit.Position = newPos;
                    break;
                case GameTiles.MagicBlock:
                    level.ToggleMagicBlock(newPos);
                    break;
                case GameTiles.SolidBlock:
                    level.ToggleSolidBlock(newPos);
                    break;
                default: break;
            }
            this.RenderScreen();
        }

        public event ItemInfoEventHandler ItemInfo;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnItemInfo(ItemInfoEventArgs e)
        {
            if (ItemInfo != null)
                ItemInfo(this, e);
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

        static SolScreenControl()
        {
            palette = new CompositePalette(paletteData, 0);
            spritePalette = new CompositePalette(spritePaletteData, 0);
        }
        #endregion
    }

    // A delegate type for hooking up change notifications.
    public delegate void ItemInfoEventHandler(object sender, ItemInfoEventArgs e);
}
