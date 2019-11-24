namespace Locksmith.Forms
{
    partial class GameScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bottomBorder = new Locksmith.Forms.BottomBorder();
            this.topBorder = new Locksmith.Forms.TopBorder();
            this.solScreen = new Locksmith.Forms.SolScreenControl();
            this.leftBorder = new Locksmith.Forms.SideBorder();
            this.rightBorder = new Locksmith.Forms.SideBorder();
            this.SuspendLayout();
            // 
            // bottomBorder
            // 
            this.bottomBorder.Location = new System.Drawing.Point(2, 218);
            this.bottomBorder.Name = "bottomBorder";
            this.bottomBorder.Patterns = null;
            this.bottomBorder.Rom = null;
            this.bottomBorder.Size = new System.Drawing.Size(258, 8);
            this.bottomBorder.SpritePatterns = null;
            this.bottomBorder.TabIndex = 10;
            this.bottomBorder.Text = "bottomBorder1";
            // 
            // topBorder
            // 
            this.topBorder.Location = new System.Drawing.Point(2, 2);
            this.topBorder.Name = "topBorder";
            this.topBorder.Patterns = null;
            this.topBorder.Rom = null;
            this.topBorder.Side = Locksmith.Forms.BorderSide.Left;
            this.topBorder.Size = new System.Drawing.Size(278, 24);
            this.topBorder.SpritePatterns = null;
            this.topBorder.TabIndex = 9;
            // 
            // solScreen
            // 
            this.solScreen.ActiveTile = Locksmith.GameTiles.None;
            this.solScreen.LevelNum = 1;
            this.solScreen.Location = new System.Drawing.Point(10, 10);
            this.solScreen.Name = "solScreen";
            this.solScreen.Patterns = null;
            this.solScreen.Rom = null;
            this.solScreen.Size = new System.Drawing.Size(240, 216);
            this.solScreen.SpritePatterns = null;
            this.solScreen.TabIndex = 8;
            this.solScreen.ItemInfo += new Locksmith.Forms.ItemInfoEventHandler(this.solScreen_ItemInfo);
            // 
            // leftBorder
            // 
            this.leftBorder.Location = new System.Drawing.Point(2, 26);
            this.leftBorder.Name = "leftBorder";
            this.leftBorder.Patterns = null;
            this.leftBorder.Rom = null;
            this.leftBorder.Side = Locksmith.Forms.BorderSide.Left;
            this.leftBorder.Size = new System.Drawing.Size(8, 192);
            this.leftBorder.SpritePatterns = null;
            this.leftBorder.TabIndex = 7;
            // 
            // rightBorder
            // 
            this.rightBorder.Location = new System.Drawing.Point(250, 26);
            this.rightBorder.Name = "rightBorder";
            this.rightBorder.Patterns = null;
            this.rightBorder.Rom = null;
            this.rightBorder.Side = Locksmith.Forms.BorderSide.Right;
            this.rightBorder.Size = new System.Drawing.Size(8, 192);
            this.rightBorder.SpritePatterns = null;
            this.rightBorder.TabIndex = 6;
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bottomBorder);
            this.Controls.Add(this.topBorder);
            this.Controls.Add(this.solScreen);
            this.Controls.Add(this.leftBorder);
            this.Controls.Add(this.rightBorder);
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(272, 245);
            this.ResumeLayout(false);

        }

        #endregion

        private BottomBorder bottomBorder;
        private TopBorder topBorder;
        private SolScreenControl solScreen;
        private SideBorder leftBorder;
        private SideBorder rightBorder;
    }
}
