namespace Locksmith.Forms
{
    partial class JumpToLevel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.loadLevelBtn = new System.Windows.Forms.Button();
            this.cancelLoadBtn = new System.Windows.Forms.Button();
            this.levelSelectBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a level to load";
            // 
            // loadLevelBtn
            // 
            this.loadLevelBtn.Location = new System.Drawing.Point(16, 56);
            this.loadLevelBtn.Name = "loadLevelBtn";
            this.loadLevelBtn.Size = new System.Drawing.Size(75, 23);
            this.loadLevelBtn.TabIndex = 2;
            this.loadLevelBtn.Text = "Load";
            this.loadLevelBtn.UseVisualStyleBackColor = true;
            this.loadLevelBtn.Click += new System.EventHandler(this.loadLevelBtn_Click);
            // 
            // cancelLoadBtn
            // 
            this.cancelLoadBtn.Location = new System.Drawing.Point(98, 56);
            this.cancelLoadBtn.Name = "cancelLoadBtn";
            this.cancelLoadBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelLoadBtn.TabIndex = 3;
            this.cancelLoadBtn.Text = "Cancel";
            this.cancelLoadBtn.UseVisualStyleBackColor = true;
            // 
            // levelSelectBox
            // 
            this.levelSelectBox.FormattingEnabled = true;
            this.levelSelectBox.Location = new System.Drawing.Point(16, 29);
            this.levelSelectBox.Name = "levelSelectBox";
            this.levelSelectBox.Size = new System.Drawing.Size(157, 21);
            this.levelSelectBox.TabIndex = 4;
            // 
            // JumpToLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 97);
            this.Controls.Add(this.levelSelectBox);
            this.Controls.Add(this.cancelLoadBtn);
            this.Controls.Add(this.loadLevelBtn);
            this.Controls.Add(this.label1);
            this.Name = "JumpToLevel";
            this.Text = "Jump To Level";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadLevelBtn;
        private System.Windows.Forms.Button cancelLoadBtn;
        private System.Windows.Forms.ComboBox levelSelectBox;
    }
}