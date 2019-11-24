using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Locksmith.Data;

namespace Locksmith.Forms
{
    public delegate void SelectLevelDelegate(int level);
    public partial class JumpToLevel : Form
    {

        public SelectLevelDelegate SelectLevel;

        public JumpToLevel()
        {
            InitializeComponent();
            for (int i = SolRom.MIN_LEVEL; i < SolRom.MAX_LEVEL - 4; i++)
                levelSelectBox.Items.Add(new LevelSelectItem(i));
            
            levelSelectBox.Items.Add(new LevelSelectItem(SolRom.LEVEL_PRINCESS, "Princess Room"));
            levelSelectBox.Items.Add(new LevelSelectItem(SolRom.LEVEL_SOLOMON, "Solomon's Room"));
            levelSelectBox.Items.Add(new LevelSelectItem(SolRom.LEVEL_HIDDEN, "Hidden Room"));
            levelSelectBox.Items.Add(new LevelSelectItem(SolRom.LEVEL_TIME, "Time"));
            levelSelectBox.Items.Add(new LevelSelectItem(SolRom.LEVEL_SPACE, "Space"));
        }

        private class LevelSelectItem {
            public string Name;
            public int Value;
            public LevelSelectItem(int value) {
                Name = value.ToString(); Value = value;
            }
            public LevelSelectItem(int value, string name) {
            Name = name; Value = value;
            }
            public override string ToString() {
            // Generates the text shown in the combo box
            return Name;
            }
        }

        private void loadLevelBtn_Click(object sender, EventArgs e)
        {
            SelectLevel((levelSelectBox.SelectedItem as LevelSelectItem).Value);
            this.Hide();
        }
    }
    
}
