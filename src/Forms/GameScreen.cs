using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Locksmith.Data;
using Romulus.Nes;

namespace Locksmith.Forms
{
    public partial class GameScreen : UserControl
    {
        public ActionMode Mode { get { return solScreen.Mode; } set { solScreen.Mode = value; } }
        public GameTiles ActiveTile { get { return solScreen.ActiveTile; } set { solScreen.ActiveTile = value; } }

        public GameScreen()
        {
            InitializeComponent();
        }

        public void LoadGameScreen(SolRom rom)
        {
            // Show main screen
            solScreen.Rom = rom;
            solScreen.RenderScreen();

            // Show borders
            topBorder.Rom = rom;
            topBorder.RenderScreen();
            bottomBorder.Rom = rom;
            bottomBorder.RenderScreen();
            leftBorder.Rom = rom;
            leftBorder.RenderScreen();
            rightBorder.Rom = rom;
            rightBorder.RenderScreen();

            DisplayCurrentLevel();
            
        }


        /// <summary>
        /// Toggles the solid blocks on the screen
        /// </summary>
        /// <returns>Returns the display state</returns>
        public bool ToggleSolidBlocks() {
            solScreen.DisplaySolidBlocks = !solScreen.DisplaySolidBlocks;
            solScreen.RenderScreen();
            return solScreen.DisplaySolidBlocks; 
        }

        /// <summary>
        /// Toggles the magic blocks on the screen
        /// </summary>
        /// <returns>Returns the display state</returns>
        public bool ToggleMagicBlocks() {
            solScreen.DisplayMagicBlocks = !solScreen.DisplayMagicBlocks;
            solScreen.RenderScreen(); 
            return solScreen.DisplayMagicBlocks;
        }

        /// <summary>
        /// Toggles the enemies on the screen
        /// </summary>
        /// <returns>Returns the display state</returns>
        public bool ToggleEnemies() {
            solScreen.DisplayEnemies = !solScreen.DisplayEnemies;
            solScreen.RenderScreen();
            return solScreen.DisplayEnemies;
        }

        public void RerenderScreen() {
            solScreen.RenderScreen();
        }

        /// <summary>
        /// Toggles the items on the screen
        /// </summary>
        /// <returns>Returns the display state</returns>
        public bool ToggleItems() { 
            solScreen.DisplayItems = !solScreen.DisplayItems; 
            solScreen.RenderScreen(); 
            return solScreen.DisplayItems;
        }

        private void DisplayCurrentLevel()
        {
            OnLevelChange(new LevelChangeEventArgs(solScreen.LevelNum));
        }

        internal void ShowNextLevel()
        {
            solScreen.ShowNextLevel();
            DisplayCurrentLevel();
        }

        internal void ShowLevel(int levelNum)
        {
            solScreen.ShowLevel(levelNum);
            DisplayCurrentLevel();
        }

        internal void ShowPrevLevel()
        {
            solScreen.ShowPrevLevel();
            DisplayCurrentLevel();
        }

        public event LevelChangeEventHandler LevelChange;
        protected virtual void OnLevelChange(LevelChangeEventArgs e)
        {
            if (LevelChange != null)
                LevelChange(this, e);
        }

        public event ItemInfoEventHandler ItemInfo;

        // A delegate type for hooking up level changes
        public delegate void LevelChangeEventHandler(object sender, LevelChangeEventArgs e);

        private void solScreen_ItemInfo(object sender, ItemInfoEventArgs e)
        {
            if (ItemInfo != null)
                ItemInfo(this, e);
        }

    }
}
