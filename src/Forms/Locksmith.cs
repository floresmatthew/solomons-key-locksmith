using System;
using System.Windows.Forms;
using Locksmith.Data;

namespace Locksmith.Forms
{
    public enum ActionMode
    {
        None,
        DragAndDrop,
        GetInfo
    }

    public partial class LocksmithForm : Form
    {
        OpenFileDialog ofd;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripEnemiesBtn;
        private ToolStripButton toolStripItemsBtn;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripButton prevLevelBtn;
        private ToolStripButton nextLevelBtn;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem openROMToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem saveROMToolStripMenuItem1;
        private ToolStripMenuItem saveROMAsToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel currentLevelLbl;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private SaveFileDialog sfd;
        private ToolStripMenuItem screenStripMenuItem1;
        private ToolStripMenuItem toggleLayersToolStripMenuItem;
        private ToolStripMenuItem hideMagicBlocksToolStripMenuItem;
        private ToolStripMenuItem hideSolidBlocksToolStripMenuItem;
        private ToolStripMenuItem hideItemsToolStripMenuItem;
        private ToolStripMenuItem hideEnemiesToolStripMenuItem;
        private ToolStripButton toolStripMagicBlockButton;
        private ToolStripButton toolStripSolidBlockButton;
        private CheckBox hiddenItemCheckbox;
        private CheckBox itemInBlockCheckbox;
        private ToolStripButton dragItemButton;
        private ToolStripButton toolStripDanaStartBtn;
        private GameScreen solKeyGameScreen;
        private ToolStripMenuItem levelToolStripMenuItem;
        private ToolStripMenuItem nextLevelToolStripMenuItem;
        private ToolStripMenuItem previousLevelToolStripMenuItem;
        private ToolStripMenuItem jumpToLevelToolStripMenuItem;
        private ToolStripMenuItem applyIPSPatchToolStripMenuItem;
        private ToolStripMenuItem saveIPSPatchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        SolRom rom;
        private ComboBox itemTypeComboBox;
        private TabControl tabControl1;
        private TabPage itemTabPage;
        private TabPage enemyTabPage;
        JumpToLevel jumpToLevel = new JumpToLevel();
        private ItemListView itemListView;
        private EnemyListView enemyListView;
        private ComboBox enemyTypeComboBox;

        public LocksmithForm() {
            InitializeComponent();

            jumpToLevel.SelectLevel = new SelectLevelDelegate(this.LevelSelected);
        }

        private void openROM_Click(object sender, System.EventArgs e)
        {
            ofd.Reset();
            ofd.Filter = "NES ROM|*.nes";
            ofd.Title = "Open Solomon\'s Key Rom";
            // Get filename
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Open rom
                rom = new SolRom(ofd.FileName);

                solKeyGameScreen.LoadGameScreen(rom.ROMData);
            }
            applyIPSPatchToolStripMenuItem.Enabled = true;
            saveIPSPatchToolStripMenuItem.Enabled = true;
        }

        private void exitMenu_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void saveROMAs_Click(object sender, System.EventArgs e)
        {
            sfd.Reset();

            sfd.Filter = "NES ROM|*.nes";
            sfd.Title = "Save ROM";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save rom
                rom.Save(sfd.FileName);
            }
        }

        // Create controls
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocksmithForm));
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripEnemiesBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripItemsBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripMagicBlockButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSolidBlockButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDanaStartBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.prevLevelBtn = new System.Windows.Forms.ToolStripButton();
            this.nextLevelBtn = new System.Windows.Forms.ToolStripButton();
            this.dragItemButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openROMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.applyIPSPatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveIPSPatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveROMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveROMAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.screenStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideMagicBlocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideSolidBlocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideEnemiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.currentLevelLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.hiddenItemCheckbox = new System.Windows.Forms.CheckBox();
            this.itemInBlockCheckbox = new System.Windows.Forms.CheckBox();
            this.itemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.itemTabPage = new System.Windows.Forms.TabPage();
            this.itemListView = new Locksmith.Forms.ItemListView();
            this.enemyTabPage = new System.Windows.Forms.TabPage();
            this.enemyTypeComboBox = new System.Windows.Forms.ComboBox();
            this.enemyListView = new Locksmith.Forms.EnemyListView();
            this.solKeyGameScreen = new Locksmith.Forms.GameScreen();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.itemTabPage.SuspendLayout();
            this.enemyTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnemiesBtn,
            this.toolStripItemsBtn,
            this.toolStripMagicBlockButton,
            this.toolStripSolidBlockButton,
            this.toolStripDanaStartBtn,
            this.toolStripSeparator8,
            this.prevLevelBtn,
            this.nextLevelBtn,
            this.dragItemButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(742, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripEnemiesBtn
            // 
            this.toolStripEnemiesBtn.CheckOnClick = true;
            this.toolStripEnemiesBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEnemiesBtn.Enabled = false;
            this.toolStripEnemiesBtn.Image = global::Locksmith.Properties.Resources.spawn;
            this.toolStripEnemiesBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEnemiesBtn.Name = "toolStripEnemiesBtn";
            this.toolStripEnemiesBtn.Size = new System.Drawing.Size(23, 22);
            this.toolStripEnemiesBtn.Text = "toolStripEnemiesButton";
            this.toolStripEnemiesBtn.ToolTipText = "Show Enemies List";
            // 
            // toolStripItemsBtn
            // 
            this.toolStripItemsBtn.CheckOnClick = true;
            this.toolStripItemsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripItemsBtn.Image = global::Locksmith.Properties.Resources.bell;
            this.toolStripItemsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemsBtn.Name = "toolStripItemsBtn";
            this.toolStripItemsBtn.Size = new System.Drawing.Size(23, 22);
            this.toolStripItemsBtn.Text = "toolStripButton2";
            this.toolStripItemsBtn.ToolTipText = "Show Items List";
            this.toolStripItemsBtn.CheckedChanged += new System.EventHandler(this.toolStripItemsBtn_CheckedChanged);
            // 
            // toolStripMagicBlockButton
            // 
            this.toolStripMagicBlockButton.CheckOnClick = true;
            this.toolStripMagicBlockButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMagicBlockButton.Image = global::Locksmith.Properties.Resources.breakableblock;
            this.toolStripMagicBlockButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMagicBlockButton.Name = "toolStripMagicBlockButton";
            this.toolStripMagicBlockButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripMagicBlockButton.Text = "toolStripButton1";
            this.toolStripMagicBlockButton.ToolTipText = "Set Magic Blocks";
            this.toolStripMagicBlockButton.Click += new System.EventHandler(this.toolStripMagicBlockButton_Click);
            // 
            // toolStripSolidBlockButton
            // 
            this.toolStripSolidBlockButton.CheckOnClick = true;
            this.toolStripSolidBlockButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSolidBlockButton.Image = global::Locksmith.Properties.Resources.solidblock;
            this.toolStripSolidBlockButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSolidBlockButton.Name = "toolStripSolidBlockButton";
            this.toolStripSolidBlockButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripSolidBlockButton.Text = "toolStripButton2";
            this.toolStripSolidBlockButton.ToolTipText = "Set Solid Blocks";
            this.toolStripSolidBlockButton.Click += new System.EventHandler(this.toolStripSolidBlockButton_Click);
            // 
            // toolStripDanaStartBtn
            // 
            this.toolStripDanaStartBtn.CheckOnClick = true;
            this.toolStripDanaStartBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDanaStartBtn.Image = global::Locksmith.Properties.Resources.dana;
            this.toolStripDanaStartBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDanaStartBtn.Name = "toolStripDanaStartBtn";
            this.toolStripDanaStartBtn.Size = new System.Drawing.Size(23, 22);
            this.toolStripDanaStartBtn.Text = "toolStripButton1";
            this.toolStripDanaStartBtn.ToolTipText = "Set Player Start Position";
            this.toolStripDanaStartBtn.Click += new System.EventHandler(this.toolStripDanaStartBtn_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // prevLevelBtn
            // 
            this.prevLevelBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.prevLevelBtn.Image = global::Locksmith.Properties.Resources.arrowback;
            this.prevLevelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.prevLevelBtn.Name = "prevLevelBtn";
            this.prevLevelBtn.Size = new System.Drawing.Size(23, 22);
            this.prevLevelBtn.Text = "toolStripButton1";
            this.prevLevelBtn.ToolTipText = "Previous Level";
            this.prevLevelBtn.Click += new System.EventHandler(this.prevLevelBtn_Click);
            // 
            // nextLevelBtn
            // 
            this.nextLevelBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextLevelBtn.Image = global::Locksmith.Properties.Resources.arrownext;
            this.nextLevelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextLevelBtn.Name = "nextLevelBtn";
            this.nextLevelBtn.Size = new System.Drawing.Size(23, 22);
            this.nextLevelBtn.Text = "toolStripButton2";
            this.nextLevelBtn.ToolTipText = "Next Level";
            this.nextLevelBtn.Click += new System.EventHandler(this.nextLevelBtn_Click);
            // 
            // dragItemButton
            // 
            this.dragItemButton.CheckOnClick = true;
            this.dragItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dragItemButton.Image = ((System.Drawing.Image)(resources.GetObject("dragItemButton.Image")));
            this.dragItemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dragItemButton.Name = "dragItemButton";
            this.dragItemButton.Size = new System.Drawing.Size(23, 22);
            this.dragItemButton.Text = "toolStripButton1";
            this.dragItemButton.ToolTipText = "Drag and Drop";
            this.dragItemButton.CheckStateChanged += new System.EventHandler(this.dragItemButton_CheckStateChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.screenStripMenuItem1,
            this.levelToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(742, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem1,
            this.toolStripSeparator10,
            this.applyIPSPatchToolStripMenuItem,
            this.saveIPSPatchToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveROMToolStripMenuItem1,
            this.saveROMAsToolStripMenuItem1,
            this.toolStripSeparator11,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // openROMToolStripMenuItem1
            // 
            this.openROMToolStripMenuItem1.Name = "openROMToolStripMenuItem1";
            this.openROMToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openROMToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.openROMToolStripMenuItem1.Text = "&Open ROM...";
            this.openROMToolStripMenuItem1.Click += new System.EventHandler(this.openROM_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(182, 6);
            // 
            // applyIPSPatchToolStripMenuItem
            // 
            this.applyIPSPatchToolStripMenuItem.Enabled = false;
            this.applyIPSPatchToolStripMenuItem.Name = "applyIPSPatchToolStripMenuItem";
            this.applyIPSPatchToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.applyIPSPatchToolStripMenuItem.Text = "Apply IPS Patch";
            this.applyIPSPatchToolStripMenuItem.ToolTipText = "Not yet";
            this.applyIPSPatchToolStripMenuItem.Click += new System.EventHandler(this.applyIPSPatch_Click);
            // 
            // saveIPSPatchToolStripMenuItem
            // 
            this.saveIPSPatchToolStripMenuItem.Enabled = false;
            this.saveIPSPatchToolStripMenuItem.Name = "saveIPSPatchToolStripMenuItem";
            this.saveIPSPatchToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveIPSPatchToolStripMenuItem.Text = "Save IPS Patch";
            this.saveIPSPatchToolStripMenuItem.ToolTipText = "Not yet";
            this.saveIPSPatchToolStripMenuItem.Click += new System.EventHandler(this.saveIPSPatch_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // saveROMToolStripMenuItem1
            // 
            this.saveROMToolStripMenuItem1.Enabled = false;
            this.saveROMToolStripMenuItem1.Name = "saveROMToolStripMenuItem1";
            this.saveROMToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveROMToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.saveROMToolStripMenuItem1.Text = "&Save ROM...";
            // 
            // saveROMAsToolStripMenuItem1
            // 
            this.saveROMAsToolStripMenuItem1.Name = "saveROMAsToolStripMenuItem1";
            this.saveROMAsToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.saveROMAsToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.saveROMAsToolStripMenuItem1.Text = "&Save ROM As...";
            this.saveROMAsToolStripMenuItem1.Click += new System.EventHandler(this.saveROMAs_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(182, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitMenu_Click);
            // 
            // screenStripMenuItem1
            // 
            this.screenStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleLayersToolStripMenuItem});
            this.screenStripMenuItem1.Name = "screenStripMenuItem1";
            this.screenStripMenuItem1.Size = new System.Drawing.Size(54, 20);
            this.screenStripMenuItem1.Text = "Screen";
            // 
            // toggleLayersToolStripMenuItem
            // 
            this.toggleLayersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideMagicBlocksToolStripMenuItem,
            this.hideSolidBlocksToolStripMenuItem,
            this.hideItemsToolStripMenuItem,
            this.hideEnemiesToolStripMenuItem});
            this.toggleLayersToolStripMenuItem.Name = "toggleLayersToolStripMenuItem";
            this.toggleLayersToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.toggleLayersToolStripMenuItem.Text = "Toggle Layers";
            // 
            // hideMagicBlocksToolStripMenuItem
            // 
            this.hideMagicBlocksToolStripMenuItem.Name = "hideMagicBlocksToolStripMenuItem";
            this.hideMagicBlocksToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.hideMagicBlocksToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.hideMagicBlocksToolStripMenuItem.Text = "Hide Magic Blocks";
            this.hideMagicBlocksToolStripMenuItem.Click += new System.EventHandler(this.hideMagicBlocksToolStripMenuItem_Click);
            // 
            // hideSolidBlocksToolStripMenuItem
            // 
            this.hideSolidBlocksToolStripMenuItem.Name = "hideSolidBlocksToolStripMenuItem";
            this.hideSolidBlocksToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.hideSolidBlocksToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.hideSolidBlocksToolStripMenuItem.Text = "Hide Solid Blocks";
            this.hideSolidBlocksToolStripMenuItem.Click += new System.EventHandler(this.hideSolidBlocksToolStripMenuItem_Click);
            // 
            // hideItemsToolStripMenuItem
            // 
            this.hideItemsToolStripMenuItem.Name = "hideItemsToolStripMenuItem";
            this.hideItemsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.hideItemsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.hideItemsToolStripMenuItem.Text = "Hide Items";
            this.hideItemsToolStripMenuItem.Click += new System.EventHandler(this.hideItemsToolStripMenuItem_Click);
            // 
            // hideEnemiesToolStripMenuItem
            // 
            this.hideEnemiesToolStripMenuItem.Name = "hideEnemiesToolStripMenuItem";
            this.hideEnemiesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.hideEnemiesToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.hideEnemiesToolStripMenuItem.Text = "Hide Enemies";
            this.hideEnemiesToolStripMenuItem.Click += new System.EventHandler(this.hideEnemiesToolStripMenuItem_Click);
            // 
            // levelToolStripMenuItem
            // 
            this.levelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextLevelToolStripMenuItem,
            this.previousLevelToolStripMenuItem,
            this.jumpToLevelToolStripMenuItem});
            this.levelToolStripMenuItem.Name = "levelToolStripMenuItem";
            this.levelToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.levelToolStripMenuItem.Text = "&Level";
            // 
            // nextLevelToolStripMenuItem
            // 
            this.nextLevelToolStripMenuItem.Name = "nextLevelToolStripMenuItem";
            this.nextLevelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
            this.nextLevelToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.nextLevelToolStripMenuItem.Text = "&Next Level";
            this.nextLevelToolStripMenuItem.Click += new System.EventHandler(this.nextLevelBtn_Click);
            // 
            // previousLevelToolStripMenuItem
            // 
            this.previousLevelToolStripMenuItem.Name = "previousLevelToolStripMenuItem";
            this.previousLevelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
            this.previousLevelToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.previousLevelToolStripMenuItem.Text = "&Previous Level";
            this.previousLevelToolStripMenuItem.Click += new System.EventHandler(this.prevLevelBtn_Click);
            // 
            // jumpToLevelToolStripMenuItem
            // 
            this.jumpToLevelToolStripMenuItem.Name = "jumpToLevelToolStripMenuItem";
            this.jumpToLevelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.jumpToLevelToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.jumpToLevelToolStripMenuItem.Text = "&Jump to Level";
            this.jumpToLevelToolStripMenuItem.Click += new System.EventHandler(this.gotoLevelMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem2});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem2
            // 
            this.aboutToolStripMenuItem2.Name = "aboutToolStripMenuItem2";
            this.aboutToolStripMenuItem2.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem2.Text = "&About";
            this.aboutToolStripMenuItem2.Click += new System.EventHandler(this.aboutToolStripMenuItem2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentLevelLbl,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 336);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(742, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // currentLevelLbl
            // 
            this.currentLevelLbl.BackColor = System.Drawing.SystemColors.Control;
            this.currentLevelLbl.Name = "currentLevelLbl";
            this.currentLevelLbl.Size = new System.Drawing.Size(77, 17);
            this.currentLevelLbl.Text = "Current Level";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // hiddenItemCheckbox
            // 
            this.hiddenItemCheckbox.AutoSize = true;
            this.hiddenItemCheckbox.Location = new System.Drawing.Point(6, 6);
            this.hiddenItemCheckbox.Name = "hiddenItemCheckbox";
            this.hiddenItemCheckbox.Size = new System.Drawing.Size(82, 17);
            this.hiddenItemCheckbox.TabIndex = 6;
            this.hiddenItemCheckbox.Text = "Hidden item";
            this.hiddenItemCheckbox.UseVisualStyleBackColor = true;
            // 
            // itemInBlockCheckbox
            // 
            this.itemInBlockCheckbox.AutoSize = true;
            this.itemInBlockCheckbox.Location = new System.Drawing.Point(6, 30);
            this.itemInBlockCheckbox.Name = "itemInBlockCheckbox";
            this.itemInBlockCheckbox.Size = new System.Drawing.Size(86, 17);
            this.itemInBlockCheckbox.TabIndex = 7;
            this.itemInBlockCheckbox.Text = "Item in block";
            this.itemInBlockCheckbox.UseVisualStyleBackColor = true;
            // 
            // itemTypeComboBox
            // 
            this.itemTypeComboBox.FormattingEnabled = true;
            this.itemTypeComboBox.Location = new System.Drawing.Point(113, 4);
            this.itemTypeComboBox.Name = "itemTypeComboBox";
            this.itemTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.itemTypeComboBox.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.itemTabPage);
            this.tabControl1.Controls.Add(this.enemyTabPage);
            this.tabControl1.Location = new System.Drawing.Point(290, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 223);
            this.tabControl1.TabIndex = 10;
            // 
            // itemTabPage
            // 
            this.itemTabPage.Controls.Add(this.itemTypeComboBox);
            this.itemTabPage.Controls.Add(this.hiddenItemCheckbox);
            this.itemTabPage.Controls.Add(this.itemListView);
            this.itemTabPage.Controls.Add(this.itemInBlockCheckbox);
            this.itemTabPage.Location = new System.Drawing.Point(4, 22);
            this.itemTabPage.Name = "itemTabPage";
            this.itemTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.itemTabPage.Size = new System.Drawing.Size(432, 197);
            this.itemTabPage.TabIndex = 0;
            this.itemTabPage.Text = "Items";
            this.itemTabPage.UseVisualStyleBackColor = true;
            // 
            // itemListView
            // 
            this.itemListView.Location = new System.Drawing.Point(16, 63);
            this.itemListView.Name = "itemListView";
            this.itemListView.Patterns = null;
            this.itemListView.Rom = null;
            this.itemListView.Size = new System.Drawing.Size(420, 110);
            this.itemListView.SpritePatterns = null;
            this.itemListView.TabIndex = 9;
            this.itemListView.Text = "itemListView1";
            // 
            // enemyTabPage
            // 
            this.enemyTabPage.Controls.Add(this.enemyTypeComboBox);
            this.enemyTabPage.Controls.Add(this.enemyListView);
            this.enemyTabPage.Location = new System.Drawing.Point(4, 22);
            this.enemyTabPage.Name = "enemyTabPage";
            this.enemyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.enemyTabPage.Size = new System.Drawing.Size(432, 197);
            this.enemyTabPage.TabIndex = 1;
            this.enemyTabPage.Text = "Enemies";
            this.enemyTabPage.UseVisualStyleBackColor = true;
            // 
            // enemyTypeComboBox
            // 
            this.enemyTypeComboBox.FormattingEnabled = true;
            this.enemyTypeComboBox.Location = new System.Drawing.Point(91, 6);
            this.enemyTypeComboBox.Name = "enemyTypeComboBox";
            this.enemyTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.enemyTypeComboBox.TabIndex = 9;
            // 
            // enemyListView
            // 
            this.enemyListView.Location = new System.Drawing.Point(3, 65);
            this.enemyListView.Name = "enemyListView";
            this.enemyListView.Patterns = null;
            this.enemyListView.Rom = null;
            this.enemyListView.Size = new System.Drawing.Size(422, 93);
            this.enemyListView.SpritePatterns = null;
            this.enemyListView.TabIndex = 0;
            this.enemyListView.Text = "enemyListView1";
            // 
            // solKeyGameScreen
            // 
            this.solKeyGameScreen.ActiveTile = Locksmith.GameTiles.None;
            this.solKeyGameScreen.Location = new System.Drawing.Point(12, 52);
            this.solKeyGameScreen.Mode = Locksmith.Forms.ActionMode.None;
            this.solKeyGameScreen.Name = "solKeyGameScreen";
            this.solKeyGameScreen.Size = new System.Drawing.Size(272, 245);
            this.solKeyGameScreen.TabIndex = 8;
            this.solKeyGameScreen.LevelChange += new Locksmith.Forms.GameScreen.LevelChangeEventHandler(this.solKeyGameScreen_LevelChange);
            this.solKeyGameScreen.ItemInfo += new Locksmith.Forms.ItemInfoEventHandler(this.solKeyGameScreen_ItemInfo);
            // 
            // LocksmithForm
            // 
            this.ClientSize = new System.Drawing.Size(742, 358);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.solKeyGameScreen);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LocksmithForm";
            this.Text = "Locksmith";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.itemTabPage.ResumeLayout(false);
            this.itemTabPage.PerformLayout();
            this.enemyTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // This function demonstrtates usage of the undoable action queue
        // Click in the blank area of the window to test.
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            return;
            //if (e.Button == MouseButtons.Left) {
            //    // Left-click to do an action.
            //    UndoExample();
            //} else {
            //    // Right-click to undo.
            //    // (Bad things happen if you right-click more than you left-click)
            //    actions.Undo();
            //}

        }
        UndoRedo actions;
        void UndoExample() {
            // This is one way to do/undo an action.
            
            // Calling 'Do' performs the specified action and adds it to the undo list
            actions.Do(
                // We're creating a new action on the spot.
                // We pass the constructor two anonymous methods: one for the do action, and one for the undo
                new UndoRedo.SolAction(
                    // Our do action
                    delegate() {
                        Text = "Do"; // Set form text to "Do"
                    },
                    // Our undo action
                    delegate() {
                        Text = "Undo"; // Set form text to "Undo"
                    },
                     "Sample")
                );

            // The above approach is 'compact'. Everything related to the action is in one place.
            // The problem is that things get complicated fast. The Do/Undo code can't just 
            // change ROM data. You also need to update the UI everytime the action is done
            // or undone.

            // An approach that is better, but requires much more work, is to create a class
            // for each type of action. You'd want a class heirarchy of actions... i.e.
            //
            // Action {
            //     GlobalEdit {
            //         ChangePalette
            //         ChangeStartingLives
            //         <etc>
            //     }
            //     LevelEdit {
            //         ScreenEdit {
            //             ChangeBgTile <--
            //             ChangeEnemy
            //         }
            //         SetStartPoint
            //         <etc>
            //         }
            //     }
            //
            // For example, ChangeBgTile inherits ScreenEdit, which inherits LevelEdit, which
            // inherits Action. This way the UI can examine the action and figure out how to update 
            // itself (you probably want to 'dispatch' the actions, so to speak). 
            // 
            // Suppose the user clicks undo, which calls the Undo function on your action queue, which
            // undoes a ChangeBgTile action. The action queue undoes the action, then passes it to
            // the form. The form sees it's a ScreenEdit, and sends it to the screen editing control.  
            // The screen editing control sees that its a ChangeBgTile, and understands that it needs 
            // to redraw the bg. We've dispatched the action and update the UI.
        }

        #region Tool Strip Actions
        private void nextLevelBtn_Click(object sender, System.EventArgs e)
        {
            solKeyGameScreen.ShowNextLevel();
        }

        private void prevLevelBtn_Click(object sender, System.EventArgs e)
        {
            solKeyGameScreen.ShowPrevLevel();
        }

        private void toolStripDanaStartBtn_Click(object sender, System.EventArgs e)
        {
            solKeyGameScreen.ActiveTile = GameTiles.Dana;
        }

        private void toolStripItemsBtn_CheckedChanged(object sender, EventArgs e)
        {
            //ItemGroupBox.Visible = (sender as ToolStripButton).Checked;
        }

        private void toolStripSolidBlockButton_Click(object sender, System.EventArgs e)
        {
            solKeyGameScreen.ActiveTile = GameTiles.SolidBlock;
        }

        private void toolStripMagicBlockButton_Click(object sender, System.EventArgs e)
        {
            solKeyGameScreen.ActiveTile = GameTiles.MagicBlock;
        }

        private void aboutToolStripMenuItem2_Click(object sender, System.EventArgs e)
        {
            new LocksmithAboutBox().Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripItem item in (sender as ToolStrip).Items)
            {
                if (item is ToolStripButton && item != e.ClickedItem)
                    (item as ToolStripButton).Checked = false;
            }
            solKeyGameScreen.ActiveTile = GameTiles.None;
            solKeyGameScreen.Mode = ActionMode.None;
        }
        #endregion

        #region Menu Strip Actions
        private void gotoLevelMenuItem_Click(object sender, System.EventArgs e)
        {
            jumpToLevel.Show();
        }

        private void LevelSelected(int level)
        {
            solKeyGameScreen.ShowLevel(level);
        }

        #endregion


        private void solKeyGameScreen_ItemInfo(object sender, ItemInfoEventArgs e)
        {
            toolStripItemsBtn.Checked = true;
            hiddenItemCheckbox.Checked = e.IsHidden;
            itemInBlockCheckbox.Checked = e.IsInBlock;
        }

        private void dragItemButton_CheckStateChanged(object sender, EventArgs e)
        {
            if ((sender as ToolStripButton).Checked)
                solKeyGameScreen.Mode = ActionMode.DragAndDrop;
            else
                solKeyGameScreen.Mode = ActionMode.None;
        }

        private void solKeyGameScreen_LevelChange(object sender, LevelChangeEventArgs e)
        {
            string levelName = e.NewLevel.ToString();
            switch (e.NewLevel)
            {
                case SolRom.LEVEL_PRINCESS:
                    levelName = "Princess Room"; break;
                case SolRom.LEVEL_HIDDEN:
                    levelName = "Hidden Room"; break;
                case SolRom.LEVEL_SPACE:
                    levelName = "Space"; break;
                case SolRom.LEVEL_TIME:
                    levelName = "Time"; break;
                case SolRom.LEVEL_SOLOMON:
                    levelName = "Solomon's Room"; break;
                default:
                    break;
            }

            this.currentLevelLbl.Text = String.Format("Current Level: {0}", e.NewLevel);
        }

        private void hideMagicBlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Checked = !solKeyGameScreen.ToggleMagicBlocks();
            
        }

        private void hideSolidBlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Checked = !solKeyGameScreen.ToggleSolidBlocks();
        }

        private void hideItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Checked = !solKeyGameScreen.ToggleItems();
        }

        private void hideEnemiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Checked = !solKeyGameScreen.ToggleEnemies();
        }

        private void saveIPSPatch_Click(object sender, EventArgs e)
        {
            ofd.Reset();

            ofd.Filter = "NES ROM|*.nes";
            ofd.Title = "Select Unmodified Solomon\'s Key Rom";
            // Get unmodified file
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SolRom baseRom = new SolRom(ofd.FileName);

                // Save IPS Patch
                IPatchManager patchMgr = new IPSManager();
                // Have to commit any changes to our in-memory copy before saving the patch.
                this.rom.CommitChanges();
                IPatch patch = patchMgr.CreatePatch(baseRom, this.rom);

                sfd.Reset();
                sfd.Filter = patchMgr.SaveFileFilter;
                sfd.Title = "Save patch file";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Save rom
                    patchMgr.Save(patch, sfd.FileName);
                }
            }
        }

        private void applyIPSPatch_Click(object sender, EventArgs e)
        {
            IPatchManager patchMgr = new IPSManager();

            ofd.Reset();

            ofd.Filter = patchMgr.SaveFileFilter;
            ofd.Title = "Select Solomon\'s Key IPS File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            // Load IPS Patch
            IPatch patchFile = patchMgr.Load(ofd.FileName);
            patchMgr.ApplyIPSPatch(this.rom, patchFile);

            solKeyGameScreen.RerenderScreen();

        }

    }


}
